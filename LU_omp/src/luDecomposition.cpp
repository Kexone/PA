#include "luDecomposition.h"

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

std::vector<std::vector<float>> LuDecomposition::operOnMat(std::vector<std::vector<float>> matA, Matrix unit)
{
	for (int k = 0; k < rows - 1; k++) {

		float* multipliers = new float[rows + k + 1];
		float upperVal = (matA[k][k] == 0) ? 1 : matA[k][k];

		for (int i = k + 1; i <rows; i++) {
			multipliers[i] = -(matA[i][k] / upperVal);
		}

		for (int y = k + 1; y < rows; y++) {
			for (int x = 0; x < rows; x++) {
				matA[y][x] = (matA[y][x] + (multipliers[y] * matA[k][x]));
				unit.set(y, x, unit.elem(y, x) + (multipliers[y] * unit.elem(k, x)));
			}
		}
	}
	if(matrixLT.getMat().empty()) {
		this->matrixLT = Matrix(rows, rows);
		this->matrixLT = unit;

	}
	return matA;
}

std::vector<std::vector<float>> LuDecomposition::operOnMatPar(std::vector<std::vector<float>> matA, Matrix unit)
{
	for (int k = 0; k < rows - 1; k++) {

		float* multipliers = new float[rows + k + 1];
		float upperVal = (matA[k][k] == 0) ? 1 : matA[k][k];

		#pragma omp parallel for
		for (int i = k + 1; i <rows; i++) {
			multipliers[i] = -(matA[i][k] / upperVal);
		}

		#pragma omp parallel for
		for (int y = k + 1; y < rows; y++) {
			for (int x = 0; x < rows; x++) {
				matA[y][x] = (matA[y][x] + (multipliers[y] * matA[k][x]));
				unit.set(y, x, unit.elem(y, x) + (multipliers[y] * unit.elem(k, x)));
			}
		}
	}
	if (matrixLT.getMat().empty()) {
		this->matrixLT = Matrix(rows, rows);
		this->matrixLT = unit;

	}
	return matA;
}

LuDecomposition::LuDecomposition(int rows)
{
	this->rows = rows;
	setMat();
	initLUMatrix();
	initPermutationMatrix();
}

void LuDecomposition::calculate(bool paralel = false)
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

	std::vector<std::vector<float>> origMat = matrix.getMat();
	Matrix unitMat = Matrix(rows, rows);
	unitMat.generateUnitMatrix();
	this->matrixU.setMat(operOnMat(origMat, unitMat));
	unitMat.generateUnitMatrix();
	this->matrixL.setMat(operOnMat(matrixLT.getMat(), unitMat));
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
