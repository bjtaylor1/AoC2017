// Day23c.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

bool isprime(int b)
{
	for(int i = 2; i < b/2; i++)
		if (b % i == 0) return false;
	return true;
}

int main()
{
	int mul = 0;
	int b = 0, c = 0, d = 0, e = 0, f = 0, g = 0, h = 0;

b = 99;
c = b;
b *= 100;
b -= -100000;
c = b;
c -= -17000;
do {
	f = 1;
	d = 2;
	do {
		e = 2;
		do {
			g = d * e - b;
			if (d*e == b)
			{
				f = 0;				
			}
			e++;
			g = e - b;
		} while(e != b);
		d++;
		g = d - b;
	} while(g != 0);
	if (f == 0)  h++;
	g = b - c;
	if (g == 0) break;
	b -= -17;
}
while(true);
line32: std::cout <<mul;
	return 0;

	//int mul = 0;
	//int a = 1, b = 0, c = 0, d = 0, e = 0, f = 0, g = 0, h = 0;

//line0: b = 99;
//line1: c = b;
//line2: if (a != 0) goto line4;
//line3: if (1 != 0) goto line8;
//line4: b *= 100;
//line5: b -= -100000;
//line6: c = b;
//line7: c -= -17000;
//line8: f = 1;
//line9: d = 2;
//line10: e = 2;
//line11: g = d;
//line12: g *= e;
//	mul++;
//line13: g -= b;
//line14: if (g != 0) goto line16;
//line15: f = 0;
//line16: e -= -1;
//line17: g = e;
//line18: g -= b;
//line19: if (g != 0) goto line11;
//line20: d -= -1;
//line21: g = d;
//line22: g -= b;
//line23: if (g != 0) goto line10;
//line24: if (f != 0) goto line26;
//line25: h -= -1;
//line26: g = b;
//line27: g -= c;
//line28: if (g != 0) goto line30;
//line29: if (1 != 0) goto line32;
//line30: b -= -17;
//line31: if (1 != 0) goto line8;
//line32: std::cout << mul;
//	return 0;

}

