#pragma once
#include "matrix.h"

class LuDecomposition
{
private:
	void setMat();
	void initLUMatrix();
	void initPermutationMatrix();
	Matrix swapCols(Matrix mat, int firstC, int secondC);
	std::vector< std::vector < float > > operOnMat(std::vector< std::vector< float >> matA, Matrix unit);
	std::vector< std::vector < float > > operOnMatPar(std::vector< std::vector< float >> matA, Matrix unit);

	Matrix matrix;
	Matrix matrixL;
	Matrix matrixLT;
	Matrix matrixU;
	Matrix matrixPerm;
	int cThreads;
	int rows;

public:
	LuDecomposition(int rows);
	void calculate(bool paralel);
	void printResults();
};
