# Presence

Presence allows you to set a custom rich presence activity on Discord.

To start, create an application on the [Discord developer page](https://discord.com/developers/applications).

- The `Application Client ID` field in Presence will be the 18 character `Client ID` displayed on your application page. The name of your application will be used as the "Playing..." status on Discord.
- The `Details` and `State` fields will be displayed underneath the name of your application in your Discord activity.
- The `Large Image` and `Small Image` fields will be the names of the images that you add to your application on the Discord application art assets page. `https://discord.com/developers/applications/<Your Application ID>/rich-presence/assets` 
- The `Large Image Text` and `Small Image Text` fields will be the text that is displayed in a bubble when you hover your mouse over the respective image in your Discord activity.
- `Show Timestamp` determines whether or not the timestamp should be displayed in your Discord activity. The timer initially starts when Presence is launched and can be reset, see below.
- `Reset Timestamp On Update` is used to reset the timestamp to start from the current time when you click `Update Presence`.

Presence will minimize to the Windows system tray when the "X" button in the top right of the window is clicked.
