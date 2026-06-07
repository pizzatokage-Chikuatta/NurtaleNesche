Language File
================================

This tutorial adds a new language entry and optional CSV files.

Language mods are different from normal folder-backed JSON mods. The language metadata lives directly in mod.json, and the text data is CSV.

Step 1
--------

Create this folder hierarchy under Mods.

~~~text
Mods\Mod_YourName\Language\Pirate English
~~~

Step 2
--------

Inside Pirate English, create this file:

~~~text
mod.json
~~~

Put this content inside it.

~~~json
{
  "experimentalApiVersion": "v1",
  "type": "language",
  "name": "Pirate English",
  "language": "Pirate English",
  "languageCode": "PIR"
}
~~~

Field meanings:

1. "type": "language" registers a language.
2. "name": readable mod name.
3. "language": name shown in the language dropdown.
4. "languageCode": code used by CSV filenames.

Step 3
--------

To override chat/system messages, create this CSV file next to mod.json:

~~~text
CSV_ChatMessage_PIR.csv
~~~

Example content:

~~~csv
ItemGet,{0} plundered {1}.,,
ItemUse,{0} uses {1}.,,
System,{0} has reached level {1}.,,
Normal,The PizzaTokage is staring at you.,,
~~~

The first column is the key. The second column is the displayed text.

Important: CSV formats are not all the same. Chat/system CSV and patroller speech CSV use different columns. Do not copy a speech row shape into CSV_ChatMessage_CODE.csv.

Step 4
--------

To override stage names or gallery-style misc CSV files, create this folder:

~~~text
MISC CSV
~~~

For stage names, create this file inside MISC CSV:

~~~text
CSV_Stage Name_PIR.csv
~~~

The suffix must match languageCode.

If languageCode is PIR, use:

~~~text
CSV_Stage Name_PIR.csv
~~~

Do not use:

~~~text
CSV_Stage Name_EN.csv
~~~

Step 5
--------

To override patroller speech, create speech CSV files directly inside MISC CSV.

Example filename:

~~~text
CSV_Speech_PIR_patroller.goblin.csv
~~~

Example content:

~~~csv
patroller.goblin,Goblin,patrol_001,Patrol,1,Arrr...
patroller.goblin,Goblin,suspicion_001,Suspicion,1,Who goes there?!
~~~

Speech CSV columns are:

1. speakerId
2. speakerDisplayName
3. speechId
4. action
5. priority
6. speech

Step 6
--------

Your folder can look like this:

~~~text
Mods\Mod_YourName\Language\Pirate English
  mod.json
  CSV_ChatMessage_PIR.csv
  MISC CSV
    CSV_Stage Name_PIR.csv
    CSV_Speech_PIR_patroller.goblin.csv
~~~

Step 7
--------

Start the game and open:

~~~text
Main Menu -> Mods -> Mod Report
~~~

Then open the language dropdown and select Pirate English.

Important CSV Rules
--------

1. Save CSV files as UTF-8.
2. Keep early test lines simple.
3. Be careful with commas inside text because CSV parsing splits by comma.
4. Match languageCode exactly in filenames.
5. Mod speech CSV files go directly inside MISC CSV, not MISC CSV\Speech.

Common Mistakes
--------

1. Creating language.json and expecting it to load. Current v1 reads language metadata from mod.json.
2. Using languageCode PIR but naming files with EN.
3. Putting speech CSV files in MISC CSV\Speech.
4. Forgetting UTF-8 encoding.
5. Adding commas in text before testing a simple line first.
