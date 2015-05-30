/*
 * FeliCa2Money
 *
 * Copyright (C) 2001-2011 Takuya Murakami
 *
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program; if not, write to the Free Software
 *  Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
 */

using System;
using System.Collections.Generic;
using System.Text;
using FelicaLib;

namespace FeliCa2Money
{
    // Account クラス
    public abstract class Account
    {
        protected string mIdent;              // 組織名
        protected string mBankId = "Felica2Money";  // 銀行ID
        protected string mBranchId = "0";   // 支店番号
        protected string mAccountId = "";   // 口座番号
        protected string mAccountName;         // アカウント名(カード名etc)

        protected TransactionList mTransactions = new TransactionList(); // 取引リスト

        protected bool mHasBalance = false; // 残高があるか
        protected int mBalance = 0; // 残高

        protected bool mIsCreditCard = false; // クレジットカードの場合に真

        public abstract void ReadTransactions();

        public string ident
        {
            get { return mIdent; }
            set { mIdent = value; }
        }

        public string bankId
        {
            get { return mBankId; }
            set { mBankId = value; }
        }

        public string branchId
        {
            get {
                if (mBranchId == "") return "0";
                return mBranchId;
            }
            set { mBranchId = value; }
        }

        public string accountName
        {
            get { return mAccountName; }
            set { mAccountName = value; }
        }
        
        public string accountId
        {
            get {
                if (mAccountId == "") return "0";
                return mAccountId;
            }
            set { mAccountId = value; }
        }

        public bool hasBalance
        {
            set { mHasBalance = value; }
            get { return mHasBalance; }
        }

        public int balance
        {
            set { mBalance = value; }
            get { return mBalance; }
        }

        public bool isCreditCard
        {
            set { mIsCreditCard = value; }
            get { return mIsCreditCard; }
        }

        public TransactionList transactions
        {
            get { return mTransactions; }
        }

        // タブ区切りの分解 (SFCPeep用)
#if false
        protected string[] ParseLine(string line)
        {
            return line.Split('\t');
        }
#endif
    }
}
