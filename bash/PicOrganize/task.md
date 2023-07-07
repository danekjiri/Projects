Write a script `organize_photos.sh` which can be called this way:
 2
 3 ```
 4 ./organize_photos.sh OUTPUT_DIRECTORY PHOTO_1.jpg PHOTO_2.jpg ...
 5 ```
 6
 7 It should copy each of the photos into `OUTPUT_DIRECTORY`
 8 under a new name in the format `YYYY-MM-DD_HH-MM-SS.jpg`
 9 according to the date/time the photo was taken.
10 If the destination file already exists,
11 try names `YYYY-MM-DD_HH-MM-SS_1.jpg`, `YYYY-MM-DD_HH-MM-SS_2.jpg`, etc.
12
13 You can determine when the photo was taken
14 according to the EXIF metadata of the JPG files using `exiftool` command.
15 Use `-printFormat '$DateTimeOriginal'` to get just the needed timestamp
16 and use `-dateFormat` option to format it accordingly --
17 it takes formatting string similar to that of `date` command.
18
19
20 You can assume that nobody modifies the files simultaneously,
21 so that you can check for the existence of a destination file
22 and perform the movement in two different steps
23 (otherwise it may be complicated by the fact
24 that `cp` in some versions returns zero exit code even if the file was skipped).
25
26
27 Test your script by calling:
28
29 ```
30 mkdir photos
31 ./organize_photos.sh photos data/*
32 ls photos
33 ```
34
35 The `ls` command prints the following files:
36
37 ```
38 2020-07-03_15-23-28.jpg
39 2020-07-25_12-37-04_1.jpg
40 2020-07-25_12-37-04_2.jpg
41 2020-07-25_12-37-04.jpg
42 2020-07-25_12-44-44.jpg
43 2020-07-25_12-51-57.jpg
44 2020-07-25_16-19-11.jpg
45 2020-07-25_16-24-59.jpg
46 2020-07-25_16-25-55.jpg
47 2020-07-25_16-25-58.jpg
48 ```
