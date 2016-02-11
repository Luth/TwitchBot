# TwitchBot

## Getting Started
- Add System.Data.SQLite as a Resource in Visual Studio
- Create a new Resrouce File in the project called `Credentials`
- Add two strings to the `Credentials.resx` :
   - `login` Your bot's login id
   - `oauth` Your bot's oauth login (see `https://twitchapps.com/tmi/`)
- Edit the `Resources.resx` file, in the Strings tab, edit the `rooms` string (a comma-seperated list)
