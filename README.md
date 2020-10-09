# Diswords - a Discord bot for playing "Words"

you may be a bit confused, so,

## What is "Words"?

"Words" is a famous russian game. You can find the rules [here](https://github.com/NedoProgrammer/NedoProgrammer/blob/master/Words.md "here").

## Ok, well, how do I start playing it?

Here's a quick GIF showing how to create a basic game:

![](https://i.imgur.com/yXlYXCt.gif)

`dw.` is the default prefix of the bot. You can always change it :D

## How do I build it?

To run this bot, you'll need to build it.
It uses .NET Core, which you can install here: https://dotnet.microsoft.com/download

1. Clone the repository: `git clone https://github.com/NedoProgrammer/Diswords.git`

2. `cd` into the project directory: `cd Diswords/Diswords`

3. Build the project: `dotnet build --configuration Release`  

When finished, you'll find the executable in `/bin/Release/netcoreapp3.1/`

If you don't want to change the code, create a `myconfig.json` file in the `/bin/Release/netcoreapp3.1/` directory and put this into it:

```json
{
    "Token": "your discord bot's token",
    "DefaultLanguage": "en",
    "DefaultPrefix": "dw.",
    "RootDirectory": "",
    "GuildsDirectoryName": "Guilds",
    "LanguagesDirectoryName": "Languages",
    "LoadingGif": "loading.gif"
}
```

## And this is the end?

No, I'll update this README with new stuff and add new features to the bot, but not now.
I also plan to upload it to some hosting service so you could play it without building and running it on your machine.
