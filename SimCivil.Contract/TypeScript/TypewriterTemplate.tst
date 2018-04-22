${
    // Enable extension methods by adding using Typewriter.Extensions.*
    using Typewriter.Extensions.Types;

    // Uncomment the constructor to change template settings.
    //Template(Settings settings)
    //{
    //    settings.
    //}

    // Custom extension methods can be used in the template by adding a $ prefix e.g. $LoudName
    string LoudName(Property property)
    {
        return property.Name.ToUpperInvariant();
    }
}
module SimCivil.Contract {

    // $Classes/Enums/Interfaces(filter)[template][separator]
    // filter (optional): Matches the name or full name of the current item. * = match any, wrap in [] to match attributes or prefix with : to match interfaces or base classes.
    // template: The template to repeat for each matched item
    // separator (optional): A separator template that is placed between all templates e.g. $Properties[public $name: $Type][, ]

    // More info: http://frhagn.github.io/Typewriter/

    $Classes(*)[
    export class $Name {
        $Properties[
        // $LoudName
        public $name: $Type = $Type[$Default];]
		$Methods[
		public $name($Parameters[$name: $Type][, ]): $Type{
			return $Type[$Default];
		}
		]

    }]

	$Enums(*)[
	export enum $Name{
		$Values[
		$name,
		]
	}	
	]

	$Interfaces(*)[
	export interface $Name{
		$Properties[
		$name: $Type; 
		]
		$Attributes[
		$name: $Value;
		]
		$Methods[
		$name($Parameters[$name: $Type][, ]): $Type;
		]
	}
	]
}