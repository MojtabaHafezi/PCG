using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ControlNetwork
{
	public NeuralNetwork Input, Output, Hidden;


	public void Initialize (int inputNodes, int outputNodes, int hiddenNodes)
	{
		Input = new NeuralNetwork ();
		Output = new NeuralNetwork ();
		Hidden = new NeuralNetwork ();
		//Initialize Input Layer
		Input.numberNodes = inputNodes;
		Input.numberChildNodes = hiddenNodes;
		Input.numberParentNodes = 0;
		Input.Initialization (inputNodes, null, Hidden);
		Input.RandomizeWeights ();

		//Initialize Hidden Layer
		Hidden.numberNodes = hiddenNodes;
		Hidden.numberChildNodes = outputNodes;
		Hidden.numberParentNodes = inputNodes;
		Hidden.Initialization (hiddenNodes, Input, Output);
		Hidden.RandomizeWeights ();

		//Initialize Output Layer
		Output.numberNodes = outputNodes;
		Output.numberChildNodes = 0;
		Output.numberParentNodes = hiddenNodes;
		Output.Initialization (outputNodes, Hidden, null);
	}

	public void GiveInput (int i, double value)
	{
		if ((i >= 0 && (i < Input.numberNodes))) {
			Input.calcOutput [i] = value;
		}
	}

	//returns the specified output neuron or int max value when the neuron doesnt exist
	public double GetOutput (int i)
	{
		if (i >= 0 && (i < Output.numberNodes)) {
			return Output.calcOutput [i];
		}
		return (double)int.MaxValue;
	}

	//index and value for the neuron is taken and set at the desired position
	public void DesiredOutput (int i, double value)
	{
		if ((i >= 0) && (i < Output.numberNodes)) {
			Output.desiredOutput [i] = value;
		}
	}
	//calculates the outputs for using the method GetOutput
	public void FeedForward ()
	{
		Input.CalculatedOutput ();
		Hidden.CalculatedOutput ();
		Output.CalculatedOutput ();
	}

	//after the output has been calculated, the weights need to be adjusted
	public void BackPropogate ()
	{
		Output.CalculateErrors ();
		Hidden.CalculateErrors ();
		Hidden.AdjustWeights ();
		Input.AdjustWeights ();
	}

	//determine which output neuron gets fired (maximum value of all output neurons)
	public int GetMaxOutput ()
	{
		int id;
		double max;
		max = Output.calcOutput [0];
		id = 0;
		for (int i = 1; i < Output.numberNodes; i++) {
			if (Output.calcOutput [i] > max) {
				max = Output.calcOutput [i];
				id = i;
			}
		}
		return id;
	}

	//returns error value: calced output - desired using the mean square error function
	public double CalculateError ()
	{
		double error = 0;
		for (int i = 0; i < Output.numberNodes; i++) {
			float value = (float)(Output.calcOutput [i] - Output.desiredOutput [i]);
			error += Mathf.Pow (value, 2f);
		}
		error = error / Output.numberNodes;
		return error;
	}

	//set the learning rate for all layers
	public void SetLearningRate (double rate)
	{
		Input.learningRate = Hidden.learningRate = Output.learningRate = rate;
	}
	//only output will use the linear activation function
	public void SetLinearRule (bool value)
	{
		Input.linear = Output.linear = Hidden.linear = value;
	}
	//activate the momentum and give the parameter
	public void SetMomentum (bool useMomentum, double factor)
	{
		Input.momentum = Output.momentum = Hidden.momentum = useMomentum;
		Input.momentumValue = Hidden.momentumValue = Output.momentumValue = factor;
	}

	public string ToStringData ()
	{
		string message = "";
		message += "Input Layer: \n";
		message += "Node Values: \n";
		for (int i = 0; i < Input.numberNodes; i++) {
			message += Input.calcOutput [i].ToString ("F2") + ";";
		}
		message += "\nWeights: \n";
		for (int i = 0; i < Input.numberNodes; i++) {
			for (int j = 0; j < Input.numberChildNodes; j++) {
				message += Input.weights [i, j].ToString ("F5") + ";";
			}
		}
		message += "\nBias Weights: \n";
		for (int j = 0; j < Input.numberChildNodes; j++) {
			message += Input.biasWeights [j].ToString ("F5") + ";";
		}
		message += "\nHidden Layer: \n";

		message += "Weights: \n";
		for (int i = 0; i < Hidden.numberNodes; i++) {
			for (int j = 0; j < Hidden.numberChildNodes; j++) {
				message += Hidden.weights [i, j].ToString ("F5") + ";";
			}
		}
		message += "\nBias Weights: \n";
		for (int j = 0; j < Hidden.numberChildNodes; j++) {
			message += Hidden.biasWeights [j].ToString ("F5") + ";";
		}

		message += "\nOutput Layer: \n";

		for (int i = 0; i < Output.numberNodes; i++) {
			message += Output.calcOutput [i].ToString ("F3") + ";";
	
		}

		//Debug.Log (message);
		return message;
	}

}
