using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputLayer : Layer
{
	public InputLayer (int number, int numberChild, int numberParent)
	{
		numberOfNodes = number;
		numberOfChildNodes = numberChild;
		numberOfParentNodes = numberParent;

	}


	public override void CalculatedOutput ()
	{
		// no implementation since the input layer has no parents
	}

	public override void AdjustWeights ()
	{
		double value;
		for (int i = 0; i < numberOfNodes; i++) {
			for (int j = 0; j < numberOfChildNodes; j++) {
				value = learningRate * child.errors [j] * calculatedOutput [i];
				//if momentum - previous epochs weight changes are also added
				if (momentum) {
					weights [i, j] += value + momentumFactor * weightChanges [i, j];
					weightChanges [i, j] = value;
					//if no momentum is used -> no need to save epoch
				} else {
					weights [i, j] += value;
				}
			}
		}
		for (int k = 0; k < numberOfChildNodes; k++) {
			biasWeights [k] += learningRate * child.errors [k] * biasValues [k];
		}
	}



	public override void CalculateErrors ()
	{
		if (parent == null) {
			for (int i = 0; i < numberOfNodes; i++) {
				errors [i] = 0.0;
			}
		}
	}

	public  string ToStringData ()
	{

		string message = "";
		message += "Input Layer: \n";
		message += "Node Values: \n";
		for (int i = 0; i < numberOfNodes; i++) {
			message += calculatedOutput [i].ToString ("F2") + ";";
		}
		message += "\nWeights: \n";
		for (int i = 0; i < numberOfNodes; i++) {
			for (int j = 0; j < numberOfChildNodes; j++) {
				message += weights [i, j].ToString ("F5") + ";";
			}
		}
		message += "\nBias Weights: \n";
		for (int j = 0; j < numberOfChildNodes; j++) {
			message += biasWeights [j].ToString ("F5") + ";";
		}

		return message;
	}
}
