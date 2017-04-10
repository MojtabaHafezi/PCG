using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputLayer : Layer
{
	public OutputLayer (int number, int numberChild, int numberParent)
	{
		numberOfNodes = number;
		numberOfChildNodes = numberChild;
		numberOfParentNodes = numberParent;

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
		//no changes for output since no weights are there
	}



	public override void CalculateErrors ()
	{
		for (int i = 0; i < numberOfNodes; i++) {
			//derivate of sigmoid function = sigm * (1-sigm)
			double value = calculatedOutput [i];
			double desiredValue = desiredOutput [i];
			errors [i] = (desiredValue - value) * value * (1.0f - value);

		}
	}

	public   string ToStringData ()
	{
		string message = "";
		message += "\nOutput Layer: \n";

		for (int i = 0; i < numberOfNodes; i++) {
			message += calculatedOutput [i].ToString ("F3") + ";";

		}

		return message;
	}
}

