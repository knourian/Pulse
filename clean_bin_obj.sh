#!/bin/bash

echo "Listing all bin and obj directories..."
mapfile -t dirList < <(find . -type d \( -name "bin" -o -name "obj" \))

if [ ${#dirList[@]} -eq 0 ]; then
    echo "No bin or obj directories found."
    exit 0
fi

for i in "${!dirList[@]}"; do
    echo "$((i+1)). ${dirList[$i]}"
done

echo ""
echo "Choose an option:"
echo "[A] Delete ALL listed directories"
echo "[N] Delete a specific directory by entering its number"
echo "[E] Exit without deleting"
read -p "Enter your choice (A/N/E): " choice

if [[ "$choice" == "A" ]]; then
    for dir in "${dirList[@]}"; do
        rm -rf "$dir"
        echo "Deleted: $dir"
    done
    echo "All directories deleted."
elif [[ "$choice" == "N" ]]; then
    read -p "Enter the number of the directory to delete: " delNum
    if [[ "$delNum" =~ ^[0-9]+$ ]] && (( delNum > 0 )) && (( delNum <= ${#dirList[@]} )); then
        rm -rf "${dirList[$((delNum-1))]}"
        echo "Deleted: ${dirList[$((delNum-1))]}"
    else
        echo "Invalid selection."
    fi
elif [[ "$choice" == "E" ]]; then
    echo "Exiting script..."
else
    echo "Invalid choice, try again."
fi
