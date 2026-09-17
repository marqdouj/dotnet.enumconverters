# dotnet.enumconverters

## Json Enum Converters for special case scenarios. I find them useful with third-party NPM Typesccript pacakges during JSInterop.

## Converters
-  `CamelCaseEnumConverter`. Converts the first char of the Enum to lower case during Json Serialize operations. Deserialize is case-insensitive.
-  `HyphenUnderscoreEnumConverter`. Manages the transition between underscores/hyphens during Json Serialize/Deserialize operations. Deserialize is case-insensitive.
-  `HyphenUnderscoreLCEnumConverter`. Manages the transition between underscores/hyphens during Json Serialize(to lower case) operations. Deserialize is case-insensitive.
-  `LowerCaseEnumConverter`.  Converts the Enum to lower case during Json Serialize operations. Deserialize is case-insensitive.

## Release Notes 
-  `10.0.0`
	- Initial release.