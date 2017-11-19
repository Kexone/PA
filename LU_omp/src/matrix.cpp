#include "matrix.h"
#include <iostream>

Matrix::Matrix()
{

}

Matrix::Matrix(int rows, int cols)
{
	this->rows = rows;
	this->cols = cols;
	fillVector();
}

void Matrix::generateMatrix()
{
	std::random_device rd;
	std::mt19937 rng(rd());
	std::uniform_real_distribution<float> distribution(0.f,50.f);
	//std::vector<float> tmp;
	for (int i = 0; i <rows; i++) {
		for (int j = 0; j < cols; j++) {
			data[i][j] = (distribution(rng));
		}
	//	data.push_back(tmp);
	//	tmp.clear();

	}
}

void Matrix::generateUnitMatrix()
{
	for (int i = 0; i < rows; i++) {
		data[i][i] = 1;
	}
}

void Matrix::fillVector()
{
	std::vector<float> tmp(rows);
	std::fill(tmp.begin(), tmp.end(), 0);
	for (int i = 0; i < cols; i++) {

		data.push_back(tmp);
	}
	tmp.clear();

}


std::vector<std::vector<float>> Matrix::getMat()
{
	return data;
}

void Matrix::setMat(std::vector<std::vector<float>> mat)
{
	data = mat;
}

std::vector<float> Matrix::row(int ind)
{
	return data[ind];
}

void Matrix::set(int indR, int indC, float value)
{
	data[indR][indC] = value;
}

float Matrix::elem(int indR, int indC)
{
	return data[indR][indC];
}

void Matrix::printMatrix()
{
	for (int i = 0; i < rows; i++) {
		printf("| ");
		for (int j = 0; j < rows; j++) {

			int precision = 2;
			printf(" %6.*lf ", precision, data[i][j]);
		}
		printf("|\n");
	}
	printf("\n");
}

void Matrix::clearMat()
{
	data.clear();
	fillVector();
}

