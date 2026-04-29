$source = "e:\Santhanavel personal\Watch-Try-On-Experience\My project\Assets\Watch Task\Script"
$dest = "e:\Santhanavel  personal\Watch-Try-On-Experience\My project\Assets\Watch Task\Script"
New-Item -ItemType Directory -Force -Path $dest
Copy-Item -Path "$source\*" -Destination $dest -Recurse -Force
