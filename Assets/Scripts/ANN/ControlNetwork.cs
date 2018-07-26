using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ControlNetwork
{
	public InputLayer Input;
	public HiddenLayer Hidden;
	public OutputLayer Output;

	public void Initialize (int inputNodes, int outputNodes, int hiddenNodes)
	{
		Input = new InputLayer (inputNodes, hiddenNodes, 0); 
		Hidden = new HiddenLayer (hiddenNodes, outputNodes, inputNodes);
		Output = new OutputLayer (outputNodes, 0, hiddenNodes);

		Input.Initialization (null, Hidden);
		Hidden.Initialization (Input, Output);
		Output.Initialization (Hidden, null);

		Input.RandomizeWeights ();
		Hidden.RandomizeWeights ();
	}

	public void GiveInput (int i, double value)
	{
		if ((i >= 0 && (i < Input.numberOfNodes))) {
			Input.calculatedOutput [i] = value;
		}
	}

	//returns the specified output neuron or int max value when the neuron doesnt exist
	public double GetOutput (int i)
	{
		if (i >= 0 && (i < Output.numberOfNodes)) {
			return Output.calculatedOutput [i];
		}
		return (double)int.MaxValue;
	}

	//index and value for the neuron is taken and set at the desired position
	public void DesiredOutput (int i, double value)
	{
		if ((i >= 0) && (i < Output.numberOfNodes)) {
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
	public void BackPropagate ()
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
		max = Output.calculatedOutput [0];
		id = 0;
		for (int i = 1; i < Output.numberOfNodes; i++) {
			if (Output.calculatedOutput [i] > max) {
				max = Output.calculatedOutput [i];
				id = i;
			}
		}
		return id;
	}

	//returns error value: calced output - desired using the mean square error function
	public double CalculateError ()
	{
		double error = 0;
		for (int i = 0; i < Output.numberOfNodes; i++) {
			float value = (float)(Output.calculatedOutput [i] - Output.desiredOutput [i]);
			error += Mathf.Pow (value, 2f);
		}
		error = error / Output.numberOfNodes;
		return error;
	}


	//set the learning rate for all layers
	public void SetLearningRate (double rate)
	{
		//only Input and Hidden Layer need the learning rate
		Input.learningRate = Hidden.learningRate = rate;
		//Output.learningRate = rate;
	}

	//change the momentum and give the parameter
	public void SetMomentum (bool useMomentum, double factor)
	{
		Input.momentum = Output.momentum = Hidden.momentum = useMomentum;
		Input.momentumFactor = Hidden.momentumFactor = Output.momentumFactor = factor;
	}

	public string ToStringData ()
	{
		string message = "";
		message += Input.ToStringData ();
		message += Hidden.ToStringData ();
		message += Output.ToStringData ();
		return message;
	}
}
