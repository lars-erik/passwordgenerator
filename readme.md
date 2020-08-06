## Algorithmic dictionary based password generator for backdoors.

### Generation

Pseudo randomly combines words from n dictionary streams
based on a configurable seed.  
Seeds are constructed of calculations based off todays date.  
Supported operators are +, -, *, /, ^ (pow) and parens.  
Integer date values can be referenced by .NET date formatting.  
For example:  

    yyyyMMdd * 2 + 5

On aug. 5th 2020 the seed would be 40401615, calculated from
20200805 * 2 + 5. From the sample dictionaries included, 
the password would then be "small-feeling".  
The random numbers selected from the seed can be taken
from a range further from the start by adding a salt string.
For instance, by adding "salt" to the generation of the 
password above, we would get "awesome-head".

Further, a suffix can be configured for the password.
By setting the suffix to "SFX", the previous password would
now be "awesome-head-SFX". The suffix can be further customized
by a "suffix formula" similar to the seed formula.
By setting it to "MMdd", the password would now be
"awesome-head-SFX85".

## Web site

The sample web site exposes passwords to a select Azure AD group.  
Secrets should be set locally with `dotnet user-secrets set` or 
by using Azure KeyVault etc. in production.

## Plugin dictionaries

Implement the `IStreamSource` interface from `PasswordGenerator`
and follow the "simple plugin with no dependencies" instructions in
[this Microsoft .NET tutorial](https://docs.microsoft.com/en-us/dotnet/core/tutorials/creating-app-with-plugin-support#simple-plugin-with-no-dependencies)