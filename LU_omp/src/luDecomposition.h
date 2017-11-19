#pragma once
#include "matrix.h"

class LuDecomposition
{
private:
	void setMat();
	void initLUMatrix();
	void initPermutationMatrix();
	Matrix swapCols(Matrix mat, int firstC, int secondC);
	void operOnMat(Matrix &matA, Matrix &unit);
	void operOnMatPar(Matrix &matA, Matrix &unit);

	Matrix matrix;
	Matrix matrixL;
	Matrix matrixLT;
	Matrix matrixU;
	Matrix matrixPerm;
	int cThreads;
	int rows;

public:
	LuDecomposition(int rows);
	void calculate();
	void calculatePar();
	void printResults();
	void clearMats();
};
