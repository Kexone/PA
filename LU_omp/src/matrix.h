#pragma once
#include <vector>
#include <random>
#include <math.h>
#include <iostream>
class Matrix
{
private:
	int rows;
	int cols;
	std::vector< std::vector < float > > data;
	void fillVector();

public:
	Matrix(int rows, int cols);
	Matrix();
	void generateMatrix();
	void generateUnitMatrix();
	std::vector< std::vector < float > > getMat();
	void setMat(std::vector<std::vector<float>> mat);
	std::vector< float > row(int ind);
	void set(int indR, int indC, float value);
	float elem(int indR, int indC);
	void printMatrix();
	void clearMat();

};


