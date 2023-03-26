using System;
using System.Collections;

//
// Vehicle 
//
class Plate {
	public bool    IsEven{ set; get; } = false;
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
	public bool     IsAvail{ get; set; } = true;
	public uint     Id     { get; set; } = 0;
	public string   Type   { get; set; } = default!;
	public Plate    Plate  { get; set; } = default!;
	public string   Color  { get; set; } = default!;
	public float    Hour   { get; set; } = default!;

	public void Set(uint id, string type, Plate plate, string color, float hour)
	{
		this.Id = id;
		this.Type = type;
		this.Plate = plate;
		this.Color = color;
		this.Hour = hour;
		this.IsAvail = false;
	}

	public override string ToString()
	{
		return $"{this.Id}\t{this.Plate}\t\t{this.Type}\t{this.Color}";
	}
}

//
// ParkingSystem
//
class ParkingSystem {
	private bool        IsAlive  = true;
	private uint        SlotSize = 0;
	private Vehicle[]   Items    = default!;
	private Stack<uint> Slots    = default!;

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

		// exit
		 if (inp == "exit") {
			this.IsAlive = false;
			return;
		 }

		// create_parking_lot
		if (inp == "create_slots")
			this.CreateSlots(input);

		// [check slots]
		if (this.SlotSize == 0) {
			Console.WriteLine("Slot does not initialized");
			return;
		}

		// park
		if (inp == "park")
			this.Park(input);

		// leave
		else if (inp == "leave")
			this.Leave(input);

		// status
		else if (inp == "status")
			this.Status(input);

		// available_slots
		else if (inp == "available_slots")
			this.Avail(input);

		// count_slots
		else if (inp == "count_slots")
			this.Count(input);

		// type_of_vehicles
		else if (inp == "type_of_vehicles")
			this.TypeOf(input);

		// registration_numbers_for_vehicles_with_odd_plate
		else if (inp == "reg_numbers_vehicles_with_odd_plate")
			this.RegPlate(input, false);

		// registration_numbers_for_vehicles_with_even_plate
		else if (inp == "reg_numbers_vehicles_with_even_plate")
			this.RegPlate(input, true);

		// registration_numbers_for_vehicles_with_colour
		else if (inp == "reg_numbers_vehicles_with_color")
			this.RegColor(input);

		// slot_numbers_for_vehicles_with_colour
		else if (inp == "slot_numbers_vehicles_with_color")
			this.SlotNumColor(input);

		// slot_numbers_for_registration_number
		else if (inp == "slot_number_reg")
			this.SlotNumReg(input);
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

		this.SlotSize = num;
		Console.WriteLine($"Created a parking lot with {num} slots");
		return;

inval:
		Console.WriteLine("Invalid input!");
	}

	private void Park(string[] input)
	{
		if (this.Slots.Count == 0) {
			Console.WriteLine("Sorry, parking lot is full");
			return;
		}

		if (input.Length != 4)
			goto inval0;

		if (input[3] != "Mobil" && input[3] != "Motor")
			goto inval0;

		if (input[2] == "")
			goto inval0;

		var slot = this.Slots.Pop();
		var plate = new Plate();

		if (!plate.Parse(input[1]))
			goto inval1;

		this.Items[slot].Set(slot, input[3], plate, input[2], 1);

		Console.WriteLine($"Allocated slot number: {slot}");
		return;

inval1:
		this.Slots.Push(slot);
inval0:
		Console.WriteLine("Invalid input!");
	}

	private void Leave(string[] input)
	{
		if (this.Slots.Count == this.SlotSize) {
			Console.WriteLine("Sorry, parking lot is empty");
			return;
		}

		if (input.Length != 2)
			goto inval0;

		uint num = 0;
		try {
			num = Convert.ToUInt32(input[1]);
		} catch (Exception) {
			goto inval0;
		}

		var found = false;
		for (uint i = 0; i < this.SlotSize; i++) {
			if (this.Items[i].Id == num) {
				found = true;
				break;
			}
		}

		if (!found) {
			Console.WriteLine($"Slot number: {num} already freed");
			return;
		}

		this.Items[num].IsAvail = true;
		this.Slots.Push(num);

		Console.WriteLine($"Slot number: {num} is free");
		return;

inval0:
		Console.WriteLine("Invalid input!");
	}

	private void Status(string[] input)
	{
		if (input.Length != 1) {
			Console.WriteLine("Invalid input!");
			return;
		}

		Console.WriteLine("Slot\tNo.\t\t\tType\tColor");
		uint count = 0;
		for (uint i = 0; i < this.SlotSize; i++) {
			var item = this.Items[i];
			if (!item.IsAvail) {
				Console.WriteLine(item);
				count++;
			}
		}

		if (count == 0)
			Console.WriteLine("No Data");
	}

	private void Avail(string[] input)
	{
		if (input.Length != 1) {
			Console.WriteLine("Invalid input!");
			return;
		}

		Console.WriteLine(this.Slots.Count);
	}

	private void Count(string[] input)
	{
		if (input.Length != 1) {
			Console.WriteLine("Invalid input!");
			return;
		}

		uint count = 0;
		for (uint i = 0; i < this.SlotSize; i++) {
			var item = this.Items[i];
			if (!item.IsAvail)
				count++;
		}

		Console.WriteLine(count);
	}

	private void TypeOf(string[] input)
	{
		if (input.Length != 2) {
			Console.WriteLine("Invalid input!");
			return;
		}

		uint count = 0;
		for (uint i = 0; i < this.SlotSize; i++) {
			var item = this.Items[i];
			if (!item.IsAvail && item.Type == input[1])
				count++;
		}

		Console.WriteLine(count);
	}

	private void RegPlate(string[] input, bool IsEven)
	{
		if (input.Length != 1) {
			Console.WriteLine("Invalid input!");
			return;
		}

		for (uint i = 0; i < this.SlotSize; i++) {
			var item = this.Items[i];
			if (!item.IsAvail && item.Plate.IsEven == IsEven ) {
				Console.WriteLine(item.Plate);
			}
		}
	}

	private void RegColor(string[] input)
	{
		if (input.Length != 2) {
			Console.WriteLine("Invalid input!");
			return;
		}

		uint count = 0;
		for (uint i = 0; i < this.SlotSize; i++) {
			var item = this.Items[i];
			if (!item.IsAvail && item.Color == input[1]) {
				Console.WriteLine(item.Plate);
				count++;
			}
		}

		if (count == 0)
			Console.WriteLine("Not Found");
	}

	private void SlotNumColor(string[] input)
	{
		if (input.Length != 2) {
			Console.WriteLine("Invalid input!");
			return;
		}

		uint count = 0;
		for (uint i = 0; i < this.SlotSize; i++) {
			var item = this.Items[i];
			if (!item.IsAvail && item.Color == input[1]) {
				Console.WriteLine(item.Id);
				count++;
			}
		}

		if (count == 0)
			Console.WriteLine("Not Found");
	}

	private void SlotNumReg(string[] input)
	{
		if (input.Length != 2) {
			Console.WriteLine("Invalid input!");
			return;
		}

		uint count = 0;
		for (uint i = 0; i < this.SlotSize; i++) {
			var item = this.Items[i];
			if (!item.IsAvail &&
					item.Plate.ToString() == input[1]) {
				Console.WriteLine(item.Id);
				count++;
			}
		}

		if (count == 0)
			Console.WriteLine("Not Found");
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
