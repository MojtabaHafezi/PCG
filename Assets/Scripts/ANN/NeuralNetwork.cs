using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NeuralNetwork
{
	//number of nodes for that specific layer
	public int numberNodes;
	public int numberChildNodes;
	public int numberParentNodes;
	//false: logistic activation, true: linear
	public bool linear;
	//use momentum to find global minima
	public bool momentum;
	public double momentumValue;
	//learning rate for weight adjustment
	public double learningRate;

	//stores the calculated output
	public double[] calcOutput;
	//in training we have desired outputs to calculate errors
	public double[] desiredOutput;
	//saves the errors for each neuron
	public double[] errors;
	//array of bias weights to each neuron
	public double[] biasWeights;
	//bias is a dummy input with either 1 or -1
	public double[] biasValues;

	//two-dimensional array of weight values connecting parent - child nodes
	public double[,] weights;
	//saves the adjustments done to the weights
	public double[,] weightChanges;

	public NeuralNetwork parent, child;

	public NeuralNetwork ()
	{
		momentumValue = 0.8;
		momentum = false;
		linear = false;
		parent = null;
		child = null;
	}

	//initialize all arrays with 0s and the bias with 1 or -1
	public void Initialization (int number, NeuralNetwork par, NeuralNetwork chi)
	{
		calcOutput = new double[numberNodes];
		desiredOutput = new double[numberNodes];
		errors = new double[numberNodes];


		if (par != null)
			this.parent = par;
		if (chi != null) {
			this.child = chi;
			weights = new double[numberNodes, numberChildNodes];
			weightChanges = new double[numberNodes, numberChildNodes];
		}
		for (int i = 0; i < numberNodes; i++) {
			calcOutput [i] = 0;
			desiredOutput [i] = 0;
			errors [i] = 0;
			if (child != null) {
				//since every parent node is connected to every child node
				for (int j = 0; j < numberChildNodes; j++) {
					weights [i, j] = 0;
					weightChanges [i, j] = 0;
				}
			}
		}
		if (child != null) {
			biasValues = new double[numberChildNodes];
			biasWeights = new double[numberChildNodes];
			for (int k = 0; k < numberChildNodes; k++) {
				biasValues [k] = 1;
				biasWeights [k] = 0;
			}
		}
	}

	//Randomize the weights between -1 and 1 - also in bias
	public void RandomizeWeights ()
	{
		int min = 0;
		int max = 201;
		int number;

		for (int i = 0; i < numberNodes; i++) {
			for (int j = 0; j < numberChildNodes; j++) {
				number = Random.Range (min, max);
				weights [i, j] = (number / 100.0) - 1;
			}
		}
		for (int k = 0; k < numberChildNodes; k++) {
			number = Random.Range (min, max);
			biasWeights [k] = (number / 100.0) - 1;
		}

	}

	//Calcute the net input of all parent nodes  + bias - use sigmoid function for output
	//if linear is activated then the output layer will return the linear activation function
	public void CalculatedOutput ()
	{
		double sum;
		if (parent != null) {
			for (int i = 0; i < numberNodes; i++) {
				sum = 0;
				for (int j = 0; j < numberParentNodes; j++) {
					sum += parent.calcOutput [j] * parent.weights [j, i];
				}
				sum += parent.biasValues [i] * parent.biasWeights [i];

				if (child == null && linear) {
					calcOutput [i] = sum;
					//Debug.Log ("calculated output" + calcOutput [i]);
				} else {
					calcOutput [i] = 1.0f / (1 + Mathf.Exp ((float)-sum));
				}


			}
		}
	}
	//calculate the errors
	public void CalculateErrors ()
	{
		double sum;
		//output layer has no children
		if (child == null) {
			for (int i = 0; i < numberNodes; i++) {
				

				//derivate of sigmoid function = sigm * (1-sigm)
				double value = calcOutput [i];
				double desiredValue = desiredOutput [i];
				errors [i] = (desiredValue - value) * value * (1.0f - value);

			}
			//input has no parent
		} else if (parent == null) {
			for (int i = 0; i < numberNodes; i++) {
				errors [i] = 0.0;
			}
			//hidden layer
		} else {
			for (int i = 0; i < numberNodes; i++) {
				sum = 0;
				for (int j = 0; j < numberChildNodes; j++) {
					sum += child.errors [j] * weights [i, j];
				}
				errors [i] = sum * calcOutput [i] * (1.0 - calcOutput [i]);
			}
		}
	}

	//Adjust the weights
	public void AdjustWeights ()
	{
		double value;
		//only hidden or input layer are changed
		if (child != null) {
			for (int i = 0; i < numberNodes; i++) {
				for (int j = 0; j < numberChildNodes; j++) {
					value = learningRate * child.errors [j] * calcOutput [i];
					//if momentum - previous epochs weight changes are also added
					if (momentum) {
						weights [i, j] += value + momentumValue * weightChanges [i, j];
						weightChanges [i, j] = value;
						//if no momentum is used -> no need to save epoch
					} else {
						weights [i, j] += value;
					}
				}
			}
			for (int k = 0; k < numberChildNodes; k++) {
				biasWeights [k] += learningRate * child.errors [k] * biasValues [k];
			}
		}
	}
		
}