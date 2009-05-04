@rem go to target folder
@pushd %1

@call BuildLibrary.bat /q
@call CopyAssemblies.bat /q