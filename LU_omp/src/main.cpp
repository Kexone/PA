// main.cpp : Defines the entry point for the console application.
//

#include <iostream>
#include "luDecomposition.h"
#include <time.h>

void run(int key);

int main()
{
	while(true)
	{
		int key;
		std::cout << "Count of Matrix rows:" << std::endl;
		std::cin >> key;
		if(key == 0)
		{
			break;
		}
		run(key);
	}

    return 0;
}

void run(int key)
{
	LuDecomposition lu = LuDecomposition(key);
	time_t timer= clock();
	lu.calculate(false);
	timer = clock() - timer;
	std::cout << "LU Decomposition tooks: " << static_cast<float>(timer) / CLOCKS_PER_SEC << " seconds." << std::endl;
	lu.printResults();
	timer = clock();
	//lu.calculate(true);
	timer = clock() - timer;
	//std::cout << "Paralel LU Decomposition tooks: " << static_cast<float>(timer) / CLOCKS_PER_SEC << " seconds." << std::endl;
}