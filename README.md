# ALP
 Alternate Language Project


 Libraries used: CsvHelper; System.Text.RegularExpressions(Regex);

1) Which programming language and version did you pick?
	C#

2) Why did you pick this programming language?
	I picked C# because it is similar to C and C++, while still being different enough for me to learn a new programming language. It also has a robust CSV parsing library which was very helpful in this assignment.

3) How your programming language chosen handles: object-oriented programming, file ingestion, conditional statements, assignment statements, loops, subprograms (functions/methods), unit testing and exception handling. If one or more of these are not supported by your programming language, indicate it as so. 
	In C#, everything is defined by some form of class. When a new instance of a class is created, memory is allocated to save it, then a reference is returned as the object. The file ingestion is a bit more complicated than C and C++ because you have to create a new StreamReader object, but the CSV helper library makes mapping values to a class extremely easy. Conditional statements, assignment statements, loops, and subprograms are more or less the same as C and C++, but you have to specify if a value could be null. The unit testing framework that C# uses is also used by other .NET languages like Java. The try-catch exception handling method is also very similar to C++.

4) List out 3 libraries you used from your programming language (if applicable) and explain what they are, why you chose them and what you used them for.
	 CsvHelper: This library reads Csv files and can use a ClassMap to map the values in the CSV file directly to a class.
	 System.Text.RegularExpressions: This is Regex in C#, it can be used to find specific pattern in a string.
	 System.Linq:

Answer the following questions (and provide a corresponding screen showing output answering them):

1) What company (oem) has the highest average weight of the phone body?
![Screenshot 2024-03-31 182150](https://github.com/Josh-Toll/ALP/assets/165319440/1a134f2d-42c4-40e5-9167-b7db22982bfb)

2) Was there any phones that were announced in one year and released in another? What are they? Give me the oem and models.
![Screenshot 2024-03-31 191733](https://github.com/Josh-Toll/ALP/assets/165319440/71240b07-7508-4034-b0d7-cb68583fe366)

3) How many phones have only one feature sensor?
![Screenshot 2024-03-31 183205](https://github.com/Josh-Toll/ALP/assets/165319440/28c36e77-eb06-4ae9-ae14-41cb563b75f5)

4) What year had the most phones launched in any year later than 1999?
![Screenshot 2024-03-31 191041](https://github.com/Josh-Toll/ALP/assets/165319440/eb364df4-c128-4232-b353-08c24eb43621)
