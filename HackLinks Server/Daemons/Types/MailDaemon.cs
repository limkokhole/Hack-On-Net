﻿using HackLinks_Server.Computers;
using HackLinks_Server.Computers.Processes;
using HackLinks_Server.Daemons.Types.Mail;
using HackLinks_Server.Files;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace HackLinks_Server.Daemons.Types {
    class MailDaemon : Daemon {
        #region Overrides

        public override string StrType => "mail";

        protected override Type ClientType => typeof(MailClient);

        public override DaemonType GetDaemonType() {
            return DaemonType.MAIL;
        }

        public override void OnStartUp() {
            LoadAccounts();
        }

        public override string GetSSHDisplayName() {
            return "Mail";
        }

        #endregion

        public MailDaemon(int pid, Printer printer, Node computer, Credentials credentials) : base(pid, printer, computer, credentials) { }

        public static readonly JObject defaultConfig = new JObject(
            new JProperty("DNS", "8.8.8.8"));

        public List<Account> accounts = new List<Account>();

        #region Load Acoounts

        public void LoadAccounts() {
            accounts.Clear();

            File accountFile = node.fileSystem.rootFile.GetFileAtPath("/mail/accounts.db");

            if (accountFile == null)
                return;

            foreach (string line in accountFile.Content.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries)) {
                string[] data = line.Split(',');

                if (data[0] != "MAILACCOUNT" || data.Length != 3)
                    return;

                accounts.Add(new Mail.Account(data[1], data[2]));
            }
        }

        #endregion

        #region UpdateAccountDatabase

        public void UpdateAccountDatabase() {
            File accountFile = node.fileSystem.rootFile.GetFileAtPath("/mail/accounts.db");
            if (accountFile == null)
                return;

            string newAccountFile = "";

            foreach (Mail.Account account in accounts) {
                newAccountFile += "MAILACCOUNT," + account.accountName + "," + account.password + "\r\n";
            }

            accountFile.Content = newAccountFile;
        }

        #endregion

        #region Add Account

        public void AddAccount(Account newAccount) {
            accounts.Add(newAccount);
            UpdateAccountDatabase();
        }

        #endregion

        #region Check Folders

        public bool CheckFolders() {
            File mailFolder = node.fileSystem.rootFile.GetFile("mail");
            if (mailFolder == null || !mailFolder.IsFolder()) {
                if (mailFolder != null)
                    mailFolder.RemoveFile();
                mailFolder = File.CreateNewFolder(node.fileSystem.fileSystemManager, node, node.fileSystem.rootFile, "mail");
            }

            File accountFile = mailFolder.GetFile("accounts.db");
            if (accountFile == null) {
                accountFile = File.CreateNewFile(node.fileSystem.fileSystemManager, node, mailFolder, "accounts.db");
            }

            File configFile = mailFolder.GetFile("config.json");
            if (configFile == null) {
                configFile = File.CreateNewFile(node.fileSystem.fileSystemManager, node, mailFolder, "config.json");
                configFile.Content = defaultConfig.ToString();
            }

            return true;
        }

        #endregion
    }
}
