# encrypt & decrypt with C#
This is an assignment I did for university back in 2018, implements C#, PHP, mySQL.

There were 4 tasks in total, of which 2 were programming-related. Task 2 is quite easier than Task 3, but feel free to read about both if you wish.

<h1>Task 2</h1>
Encrypts or decrypts messages by shifting the alphabet by -25 to 25 characters, depending on
what is set in the shift by field.
Allows to choose where to save an encrypted message or what file to open and decrypt a
message from.
The encryption/decryption is easily achieved by converting a message’s characters to their
ASCII codes and performing checks on the arrays containing them.

<h1>Task 3</h1>
Checks if a keyword for today is set in the remote database, if not then a new one must be
set, otherwise a login screen appears.
Setting a new keyword is achieved by loading poems from the poems table located on the
remote database, then splitting them accordingly in the combobox and listbox fields, from
which the user can choose which word to set as daily keyword.
Additional poems can also be added, from the Add Poems form.
After setting the new keyword, every agent on the agents table will be emailed the generated
number set and the date it is valid for.
When logging in, keyword and agent initials must match what is stored on the database.
There are also options to delete the keyword of the day or to add a new agent (initials +
email required).
Once logged in, messages can be encrypted/decrypted with the daily keyword and each
operation is recorded on the remote database.
The combobox allows to load old encryptions/decryptions and to perform new
encryptions/decryptions using the old keyword that was loaded from an old message.
There are also options for saving an encrypted message or opening and decrypting one from local files.



<h2>Check the ReadMe.pdf for full documentation.</h2>
