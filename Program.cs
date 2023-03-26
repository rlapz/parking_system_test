using System;
using System.Collections;

//
// Vehicle 
//
class Plate {
	private bool   IsEven = false;
	private string Prefix = "";
	private string Middle = "";
	private string Suffix = "";

	public bool Parse(string input)
	{
		var arr = input.Split("-");
		if (arr.Length != 3)
			return false;

		uint num = 0;
		try {
			num = Convert.ToUInt32(arr[1]);
		} catch (Exception) {
			return false;
		}

		if ((num & 1) == 0)
			this.IsEven = true;

		this.Prefix = arr[0];
		this.Middle = arr[1];
		this.Suffix = arr[2];
		return true;
	}

	public override string ToString()
	{
		return $"{this.Prefix}-{this.Middle}-{this.Suffix}";
	}
}

class Vehicle {
	public uint     Id   { get; set; } = default!;
	public string   Type { get; set; } = default!;
	public Plate    Plate{ get; set; } = default!;
	public string   Color{ get; set; } = default!;
	public double   Price{ get; set; } = default!;
	public DateTime Timer{ get; set; } = default!;

	public Vehicle()
	{
	}

	public override string ToString()
	{
		return $"{this.Id} {this.Plate} {this.Type} {this.Color}";
	}
}

//
// ParkingSystem
//
class ParkingSystem {
	private bool        IsAlive = true;
	private Vehicle[]   Items = default!;
	private Stack<uint> Slots = default!;

	public void GetInput()
	{
		while (this.IsAlive) {
			Console.Write("$ ");
			var input = Console.ReadLine();
			if (input == null)
				continue;

			Console.WriteLine("You inputted: " + input);
			this.ExecInput(input.Split(" "));
		}
	}

	private void ExecInput(string[] input)
	{
		var inp = input[0];

		// create_parking_lot
		if (inp == "create_slots")
			this.CreateSlots(input);

		// park
		else if (inp == "park")
			return;

		// leave
		else if (inp == "leave")
			return;

		// status
		else if (inp == "status")
			return;

		// available_slots
		else if (inp == "available_slots")
			return;

		// count_slots
		else if (inp == "count_slots")
			return;

		// type_of_vehicles
		else if (inp == "type_of_vehicles")
			return;

		// registration_numbers_for_vehicles_with_odd_plate
		else if (inp == "reg_numbers_vehicles_with_odd_plate")
			return;

		// registration_numbers_for_vehicles_with_even_plate
		else if (inp == "reg_numbers_vehicles_with_even_plate")
			return;

		// registration_numbers_for_vehicles_with_colour
		else if (inp == "reg_numbers_vehicles_with_color")
			return;

		// slot_numbers_for_vehicles_with_colour
		else if (inp == "slot_numbers_vehicles_with_color")
			return;

		// slot_numbers_for_registration_number
		else if (inp == "slot_number_reg")
			return;

		// exit
		else if (inp == "exit")
			this.IsAlive = false;
	}

	private void CreateSlots(string[] input)
	{
		var len = input.Length;
		if (len != 2)
			goto inval;

		uint num = 0;
		try {
			num = Convert.ToUInt32(input[1]);
		} catch (Exception) {
			goto inval;
		}

		this.Items = new Vehicle[num +1];
		this.Slots = new Stack<uint>((int)num);

		// fill the stack and initialize the items
		for (uint i = num; i > 0; i--) {
			this.Slots.Push(i -1);
			this.Items[i -1] = new Vehicle();
		}

		Console.WriteLine($"Created a parking lot with {num} slots");
		return;

inval:
		Console.WriteLine("Invalid input!");
	}

	private void Park()
	{
		if (this.Slots.Count == 0)
			return;

		var v = this.Slots.Pop();
		Console.WriteLine(this.Items[v].Timer);
	}

	private void Leave()
	{
		if (this.Slots.Count == 0)
			return;
	}
}


//
// Main
//
class Program {
	public static void Main()
	{
		var pk = new ParkingSystem();
		pk.GetInput();
	}
}
