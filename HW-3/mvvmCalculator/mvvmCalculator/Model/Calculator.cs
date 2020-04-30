using System;

namespace mvvmCalculator.Model
{
    public class Calculator
    {
		public double A { get; set; }
		public double B { get; set; }

		public double Add() => A + B;
		public double Sub() => A - B;
		public double Mul() => A * B;
		public double Del() => A / B;
		public double Power() => Math.Pow(A, B);
		public double Sqrt() => Math.Sqrt(A);
	}
}
