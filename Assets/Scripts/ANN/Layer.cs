using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Layer
{
	public int numberOfNodes;
	public int numberOfChildNodes;
	public int numberOfParentNodes;

	//use momentum to find global minima
	public bool momentum;
	public double momentumFactor;

	//learning rate for weight adjustment
	public double learningRate;

	//stores the calculated output
	public double[] calculatedOutput;
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

	// the connection to the other Layers
	public Layer parent, child;

	//initialize all arrays with 0s and the bias with 1 or -1
	public void Initialization (Layer par, Layer chi)
	{
		calculatedOutput = new double[numberOfNodes];
		desiredOutput = new double[numberOfNodes];
		errors = new double[numberOfNodes];

		momentum = true;
		momentumFactor = 0.75;

		if (par != null)
			this.parent = par;
		if (chi != null) {
			this.child = chi;
			weights = new double[numberOfNodes, numberOfChildNodes];
			weightChanges = new double[numberOfNodes, numberOfChildNodes];
		}
		for (int i = 0; i < numberOfNodes; i++) {
			calculatedOutput [i] = 0;
			desiredOutput [i] = 0;
			errors [i] = 0;
			if (child != null) {
				//since every parent node is connected to every child node
				for (int j = 0; j < numberOfChildNodes; j++) {
					weights [i, j] = 0;
					weightChanges [i, j] = 0;
				}
			}
		}
		if (child != null) {
			biasValues = new double[numberOfChildNodes];
			biasWeights = new double[numberOfChildNodes];
			for (int k = 0; k < numberOfChildNodes; k++) {
				biasValues [k] = 1;
				biasWeights [k] = 0;
			}
		}
	}
		
	//Calcute the net input of all parent nodes  + bias - use sigmoid function for output
	public virtual void CalculatedOutput ()
	{
	}

	public virtual void CalculateErrors ()
	{
	}

	public virtual void AdjustWeights ()
	{
	}

}
