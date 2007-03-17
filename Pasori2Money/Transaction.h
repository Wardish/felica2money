/*
 * MoneyImport : Convert Bank csv file to MS Money OFX file.
 *
 * Copyright (c) 2001-2003 Takuya Murakami. All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 *
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 *
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 * ``AS IS'' AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
 * A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE REGENTS OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 * EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
 * PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * $Id$
 */
#ifndef _TRANSACTION_H
#define _TRANSACTION_H

//
// 取引種類
//
typedef enum {
	// 入金
	T_INT=0,	// 利息
	T_DIV,		// 配当
	T_DIRECTDEP,	// 振込入金、取立入金、自動引落戻し入金
	T_DEP,		// その他入金

	// 出金
	T_PAYMENT,	// 自動引き落とし
	T_CASH,		// 現金引き出し
	T_ATM,		// カードによる引き出し
	T_CHECK,	// 小切手関連取引
	T_DEBIT,	// その他出金
} trntype;

//
// 取引種類名 (上の値と順序一致すること)
//
#ifdef DEFINE_TRNNAME
const char *trnname[] = {
	"INT", "DIV", "DIRECTDEP", "DEP",
	"PAYMENT", "CASH", "ATM", "CHECK", "DEBIT"
};
#endif

//
// 取引種類変換表
//
struct trntable {
	const char	*key;
	trntype		type;
};

#define	T_INCOME	0
#define	T_OUTGO		1

//
// 日付情報
//
typedef struct {
	int year;
	int month;
	int date;
	int hour;
	int minutes;
	int seconds;
} DateTime;


//
// トランザクションデータ
//
class Transaction {
    public:
	Transaction	*next;

	DateTime	date;		// 日付
	unsigned long	id; 		// ID
	AnsiString	desc;         	// 説明
	trntype		type;		// 種別
	long		value;		// 金額
	long		balance;	// 残高
	
	Transaction(void) { next = NULL; }
	void SetTransactionType(const char *desc, int type);

	const char *GetTrnTypeStr(void);
};

//
// トランザクション管理クラス
//   pure virtual なクラス。各銀行毎に派生させて使用する。
//
class Card;
class TransactionList {
    private:
	Transaction	*head, *tail, *pos;
	int prev_key, serial;
        AnsiString SFCPeepPath;

	virtual const char *Ident(void) = 0;
	virtual Transaction *GenerateTransaction(int nrows, char **rows, int *err) = 0;

    public:
	inline TransactionList(void) { head = tail = 0; prev_key = serial = 0; }
	~TransactionList();
	int ParseLines(TStringList *lines);

	int GenerateTransactionId(int key);

	inline Transaction *Tail(void) { return tail; }
	inline Transaction *Head(void) { pos = head; return head; }
	inline Transaction *Next(void) {
                if (pos == NULL) return NULL;
		Transaction *r = pos;
		pos = pos->next;
		return r;
	}
};

// ユーティリティ関数
AnsiString utf8(char *sjis);

#endif

