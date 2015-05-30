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
using System.Collections;

namespace FeliCa2Money
{
    public enum TransType
    {
        Int,    // 利息
        Div,    // 配当
        DirectDep,  // 振り込み入金、取り立て入金、自動引き落とし戻し入金
        Dep,    // その他入金

        Payment,
        Cash,
        ATM,
        Check,
        Debit       // その他出金
    }

    public class Transaction
    {
        public const int UNASSIGNED_ID = -1;

        private int mId = UNASSIGNED_ID; // ID
        private DateTime mDate;
        private TransType mType;        // トランザクションタイプ
        private string mDesc = "";
        private string mMemo = "";
        private int mValue = 0;      // 金額
        private int mBalance = 0;    // 残高

        // 取引ID生成用のシリアル番号。mId が UNASSIGNED_ID の場合にのみ使用
        private int mSerial = 0; 

        private bool mValid = true;

        private static Hashtable mTransIncome;
        private static Hashtable mTransOutgo;
        private static Hashtable mTransStrings;

        private static System.Security.Cryptography.MD5 sMd5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

        // プロパティ
        public int id
        {
            get { return this.mId; }
            set { mId = value; }
        }
        public DateTime date
        {
            get { return mDate; }
            set { mDate = value; }
        }
        public TransType type
        {
            get { return mType; }
            set { mType = value; }
        }
        public string desc
        {
            get { return mDesc; }
            set { mDesc = value; }
        }
        public string memo
        {
            get { return mMemo; }
            set { mMemo = value; }
        }
        public int value
        {
            get { return mValue; }
            set { mValue = value; }
        }
        public int balance
        {
            get { return mBalance; }
            set { mBalance = value; }
        }
        public int serial
        {
            get { return mSerial; }
            set { mSerial = value; }
        }

        public bool isIdUnassigned()
        {
            return (mId == UNASSIGNED_ID);
        }
    

        // 初期化
        static Transaction()
        {
            // initialize
            mTransStrings = new Hashtable();
            mTransStrings[TransType.Int] = "INT";
            mTransStrings[TransType.Div] = "DIV";
            mTransStrings[TransType.DirectDep] = "DIRECTDEP";
            mTransStrings[TransType.Dep] = "DEP";
            mTransStrings[TransType.Payment] = "PAYMENT";
            mTransStrings[TransType.Cash] = "CASH";
            mTransStrings[TransType.ATM] = "ATM";
            mTransStrings[TransType.Check] = "CHECK";
            mTransStrings[TransType.Debit] = "DEBIT";

            mTransIncome = new Hashtable();
            mTransIncome["利息"] = TransType.Int;
            mTransIncome["振込"] = TransType.DirectDep;
            mTransIncome["ﾁｬｰｼﾞ"]= TransType.DirectDep;  // Edy チャージ
            mTransIncome["入金"] = TransType.DirectDep;    // Suica チャージ

            mTransOutgo = new Hashtable();
            mTransOutgo["ＡＴＭ"] = TransType.ATM;
            mTransOutgo["ATM"]    = TransType.ATM;
        }

        public string GetTransString()
        {
            return (string)mTransStrings[type];
        }

        public void GuessTransType(bool isIncome)
        {
            Hashtable h = mTransOutgo;

            if (isIncome)
            {
                h = mTransIncome;
            }

            foreach (string key in h.Keys)
            {
                if (desc != null && desc.Contains(key))
                {
                    type = (TransType)h[key];
                    return;
                }
            }

            // no match
            if (isIncome)
            {
                type = TransType.Dep;
            }
            else
            {
                type = TransType.Debit;
            }
        }

        public void Invalidate()
        {
            mValid = false;
        }

        public static bool isInvalid(Transaction t)
        {
            return !t.mValid;
        }

        public static bool isZeroTransaction(Transaction t)
        {
            return t.value == 0;
        }

        //
        // トランザクションID文字列の生成
        //
        public string transId()
        {
            string tid;
            if (!isIdUnassigned())
            {
                /* トランザクションの ID は日付と取引番号で生成 */
                tid = String.Format("{0:0000}{1:00}{2:00}", mDate.Year, mDate.Month, mDate.Day);
                tid += String.Format("{0:0000000}", mId);
            }
            else
            {
                /* 日付とハッシュで生成 */
                tid = makeHash();
            }
            return tid;
        }

        // トランザクションの hash を生成する
        private string makeHash()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0};{1};{2};", mDate.Year, mDate.Month, mDate.Day);
            sb.AppendFormat("{0};{1};{2};{3}", mSerial, mValue, mDesc, mMemo);

            // MD5 ハッシュを計算
            byte[] hash = sMd5.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));

            // 16進文字列に変換
            StringBuilder result = new StringBuilder();
            foreach (byte b in hash)
            {
                result.Append(b.ToString("x2"));
            }
            return result.ToString();
        }
    }
}
