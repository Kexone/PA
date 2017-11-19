#include "luDecomposition.h"
#include <omp.h>
void LuDecomposition::setMat()
{
	matrix = Matrix(rows, rows);
	this->matrix.generateMatrix();
}

void LuDecomposition::initLUMatrix()
{
	matrixL = Matrix(rows, rows);
	matrixU = Matrix(rows, rows);
}

void LuDecomposition::initPermutationMatrix()
{
	matrixPerm = Matrix(rows, rows);
	this->matrixPerm.generateUnitMatrix();
}

Matrix LuDecomposition::swapCols(Matrix mat, int firstC, int secondC)
{
	std::cout << "PICI" << std::endl;
	for (int i = 0; i < rows; i++) {
		for (int j = 0; j < rows; j++) {
			if (j == firstC || j == secondC) {
				float tmp = mat.elem(i, firstC);
				mat.set(i, firstC, mat.elem(i, secondC));
				mat.set(i, secondC, tmp);
				break;
			}
		}
	}
	return mat;
}

void LuDecomposition::operOnMat(Matrix &mat, Matrix &unit)
{
	std::vector<std::vector<float>> matdata = mat.getMat();
	std::vector<std::vector<float>> unitdata = unit.getMat();
	for (int k = 0; k < rows - 1; k++) {

		float* multipliers = new float[rows + k + 1];
		float upperVal = (matdata[k][k] == 0) ? 1 : matdata[k][k];

		for (int i = k + 1; i <rows; i++) {
			multipliers[i] = -(matdata[i][k] / upperVal);
		}

		for (int y = k + 1; y < rows; y++) {
			for (int x = 0; x < rows; x++) {
				matdata[y][x] = (matdata[y][x] + (multipliers[y] * matdata[k][x]));
				unitdata[y][x] = unitdata[y][x] + (multipliers[y] * unitdata[k][x]);
			}
		}
	}
	mat.setMat(matdata);
	unit.setMat(unitdata);
}

void LuDecomposition::operOnMatPar(Matrix &mat, Matrix &unit)
{
	std::vector<std::vector<float>> matdata = mat.getMat();
	std::vector<std::vector<float>> unitdata = unit.getMat();
	for (int k = 0; k < rows - 1; k++) {

		float *multipliers =  new float[rows + k + 1];
		float upperVal = (matdata[k][k] == 0) ? 1 : matdata[k][k];

		#pragma omp parallel for shared(upperVal)
		for (int i = k + 1; i <rows; i++) {
			multipliers[i] = -(matdata[i][k] / upperVal);
		}

		#pragma omp parallel for shared(multipliers)
		for (int y = k + 1; y < rows; y++) {
			for (int x = 0; x < rows; x++) {
				matdata[y][x] = (matdata[y][x] + (multipliers[y] * matdata[k][x]));
				unitdata[y][x] = unitdata[y][x] + (multipliers[y] * unitdata[k][x]);
			}
		}
	}
	mat.setMat(matdata);
	unit.setMat(unitdata);
}

LuDecomposition::LuDecomposition(int rows)
{
	this->rows = rows;
	setMat();
	initLUMatrix();
	initPermutationMatrix();
}

void LuDecomposition::calculate()
{
	for (int i = 0; i < rows; i++) {
		if (matrix.row(i)[i] == 0) {
			for (int j = 0; j < rows; j++) {
				if (matrix.row(i)[j] != 0) {
					matrix = swapCols(matrix, i, j);
					matrixPerm = swapCols(matrixPerm, i, j);
				}
			}
		}
	}
	Matrix unitMat = Matrix(rows, rows);
	unitMat.generateUnitMatrix();

	matrixLT = Matrix(rows, rows);
	matrixLT.generateUnitMatrix();
	matrixU.setMat(matrix.getMat());

	operOnMat(matrixU, matrixLT);
	operOnMat(matrixLT, unitMat);

	matrixL = unitMat;

}

void LuDecomposition::calculatePar()
{
	#pragma omp parallel for
	for (int i = 0; i < rows; i++) {
		if (matrix.row(i)[i] == 0) {
			for (int j = 0; j < rows; j++) {
				if (matrix.row(i)[j] != 0) {
					matrix = swapCols(matrix, i, j);
					matrixPerm = swapCols(matrixPerm, i, j);
				}
			}
		}
	}
	Matrix unitMat = Matrix(rows, rows);
	unitMat.generateUnitMatrix();

	matrixLT = Matrix(rows, rows);
	matrixLT.generateUnitMatrix();
	matrixU.setMat(matrix.getMat());
//	matrixU.printMatrix();

	operOnMatPar(matrixU, matrixLT);
	//matrixU.printMatrix();
	operOnMatPar(matrixLT, unitMat);
	
	matrixL = unitMat;

}

void LuDecomposition::printResults()
{
	printf("__________ \nPermutation Matrix\n");
	this->matrixPerm.printMatrix();
	printf("__________ \nLower Matrix\n");
	this->matrixL.printMatrix();
	printf("__________ \nUper Matrix\n");
	this->matrixU.printMatrix();
	printf("__________ \nGenerated Matrix\n");
	matrix.printMatrix();
}

void LuDecomposition::clearMats()
{
	matrixLT.clearMat();
	matrixL.clearMat();
	matrixU.clearMat();
}
