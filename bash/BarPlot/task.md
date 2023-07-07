Write a shell script for drawing a labeled barplot. The user would provide data in the following format:

12  First label
120 Second label
1 Third label

The script will print graph like this:

First label (12)   | #
Second label (120) | #######
Third label (1)    |

The script will accept input filename as the first argument and will adjust the width of the output to the current screen width. It will also align the labels as can be seen in the plot above.

You can safely assume that the input file will always exist and that it will be possible to read it multiple times. No other arguments need to be recognized.
Hints

Screen width is stored in the variable $COLUMNS. Default to 80 if the variable is not set. (You can assume it will be either empty (not set) or contain a valid number).

The plot should be scaled to fill the whole width of the screen (i.e. scaled up or down).

You can squeze all consecutive spaces to one (even for labels), the first and second column are separated by space(s).

See what wc -L does.

Note that the first tests use labels of the same length to simplify writing the first versions of the script.

Consider using printf for printing the aligned lables.

The following ensures that bc computes with fractional numbers but the result is displayed as an integer (which is useful for further shell computations).

echo 'scale=0; (5 * 2.45) / 1' | bc -l

Examples

2 Alpha
4 Bravo
# COLUMNS=20
Alpha (2) | ####
Bravo (4) | ########

2 Alpha
4 Bravo
16 Delta
# COLUMNS=37
Alpha (2)  | ###
Bravo (4)  | ######
Delta (16) | ########################

