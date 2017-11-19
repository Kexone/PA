// main.cpp : Defines the entry point for the console application.
//

#include <iostream>
#include "luDecomposition.h"
#include <time.h>

void run(int key,int print);

int main()
{
	while(true)
	{
		int key, print;
		std::cout << "Count of Matrix rows and verbose:  [int, int]" << std::endl;
		std::cin >> key >> print;
		if(key == 0)
		{
			break;
		}
		run(key, print);
	}

    return 0;
}

void run(int key, int print)
{
	LuDecomposition lu = LuDecomposition(key);
	time_t timer= clock();
	lu.calculate();
	timer = clock() - timer;
	std::cout << "LU Decomposition took: " << static_cast<float>(timer) / CLOCKS_PER_SEC << " seconds." << std::endl;
	timer = clock();
	lu.clearMats();
	lu.calculatePar();
	timer = clock() - timer;
	std::cout << "Paralel LU Decomposition took: " << static_cast<float>(timer) / CLOCKS_PER_SEC << " seconds." << std::endl;
	if(print == 1)
		lu.printResults();
}