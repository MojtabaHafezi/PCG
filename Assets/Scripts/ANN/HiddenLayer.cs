using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenLayer : Layer
{
	public HiddenLayer (int number, int numberChild, int numberParent)
	{
		numberOfNodes = number;
		numberOfChildNodes = numberChild;
		numberOfParentNodes = numberParent;

	}

	//Randomize the weights between -1 and 1 - also in bias
	public  void RandomizeWeights ()
	{
		int min = 0;
		int max = 201;
		int number;

		for (int i = 0; i < numberOfNodes; i++) {
			for (int j = 0; j < numberOfChildNodes; j++) {
				number = Random.Range (min, max);
				weights [i, j] = (number / 100.0) - 1;
			}
		}
		for (int k = 0; k < numberOfChildNodes; k++) {
			number = Random.Range (min, max);
			biasWeights [k] = (number / 100.0) - 1;
		}

	}

	public override void CalculatedOutput ()
	{
		double sum;


		for (int i = 0; i < numberOfNodes; i++) {
			sum = 0;
			for (int j = 0; j < numberOfParentNodes; j++) {
				sum += parent.calculatedOutput [j] * parent.weights [j, i];
			}
			sum += parent.biasValues [i] * parent.biasWeights [i];
			calculatedOutput [i] = 1.0f / (1 + Mathf.Exp ((float)-sum));
		}
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
		double sum;
		for (int i = 0; i < numberOfNodes; i++) {
			sum = 0;
			for (int j = 0; j < numberOfChildNodes; j++) {
				sum += child.errors [j] * weights [i, j];
			}
			//derivate of sigmoid function = sigm * (1-sigm)
			errors [i] = sum * calculatedOutput [i] * (1.0 - calculatedOutput [i]);
		}
	}

	public  string ToStringData ()
	{
		
		string message = "";
		message += "\nHidden Layer: \n";

		message += "Weights: \n";
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

