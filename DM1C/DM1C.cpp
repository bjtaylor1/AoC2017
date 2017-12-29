// DM1C.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
using namespace std;

int main()
{
	string input;
	vector<int> numbers;	
	while (getline(cin, input) && !input.empty())
	{
		istringstream iss(input);
		int n;
		while (iss >> n)
		{
			numbers.push_back(n);
		}
	}
	auto start = chrono::high_resolution_clock::now();

	int numSleighs = sqrt(numbers.size());
	if (numSleighs * numSleighs != numbers.size()) throw runtime_error("Not a square number of inputs");
	vector<map<int,int>> sleighs(numSleighs);
	generate(sleighs.begin(), sleighs.end(), [sleighNum = 0, numColours = numSleighs, numbers] () mutable {
		map<int, int> sleigh;
		for (int colour = 0; colour < numColours; colour++)
		{
			sleigh.insert(pair<int, int>(colour, numbers[sleighNum * numColours + colour]));
		}
		sleighNum++;
		return sleigh;
	});
	vector<int> permutation(10);
	iota(permutation.begin(), permutation.end(), 0);

	vector<vector<int>> permutations;
	int pCount = 0;
	do
	{
		permutations.push_back(vector<int>(permutation.begin(), permutation.end()));
		//int numMoves = accumulate(permutations.begin(), permutations.end(), 0, [sleighs](int total, int sleighNum)
		//{
		//	return total + accumulate(sleighs.begin(), sleighs.end(), 0, 
		//	[innerSleighNum = 0, sleighNum] (int subTotal, map<int, int> sleigh) mutable 
		//	{
		//		return subTotal + (innerSleighNum++ == sleighNum ? 0 : sleigh[sleighNum]);
		//	});
		//});
		//if (numMoves < minNumMoves) minNumMoves = numMoves;
	} while (next_permutation(permutation.begin(), permutation.end()));

	
	auto intermediate1 = chrono::high_resolution_clock::now();
	cout << "Calculated " << permutations.size() << " permutations (" << ((chrono::duration<double>)(intermediate1 - start)).count() << ") " << endl;

	int minNumMoves = accumulate(numbers.begin(), numbers.end(), 0);
	for (auto permutation : permutations)
	{
		int numMoves = 0;
		for (int sleighNumOuter = 0; sleighNumOuter < numSleighs; sleighNumOuter++)
		{
			int colour = permutation[sleighNumOuter];
			int sleighNumInner = 0;
			auto moves = accumulate(sleighs.begin(), sleighs.end(), 0, [sleighNumInner = 0, sleighNumOuter, colour](int total, map<int, int> sleigh) mutable
			{
				if (sleighNumInner++ != sleighNumOuter)
				{
					return total + sleigh[colour];
				}
				else return total;
			});
			numMoves += moves;
		}
		if (numMoves < minNumMoves) minNumMoves = numMoves;
	}
	auto end = chrono::high_resolution_clock::now();
	
	cout << minNumMoves << " (" << ((chrono::duration<double>)(end - start)).count() << ") " << endl;
	cin.get();
    return 0;
}

