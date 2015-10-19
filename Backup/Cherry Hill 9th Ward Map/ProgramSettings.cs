using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;


namespace Cherry_Hill_9th_Ward_Map
{
   public class ProgramSettings
   {
/*    public const string UPGRADE_PROMPT_ON_SUCCESS = "Prompt For Upgrade On Success";
      public const string UPGRADE_PROMPT_ALWAYS = "Prompt For Upgrade Always";
      public const string UPGRADE_ON_SUCCESS = "Upgrade On Success";
      public const string UPGRADE_DISABLED = "Upgrade Disabled";
      public const string DEFAULT_DATABASE_NAME = "manTestResults";
      public const string DEFAULT_DB_SERVER_NAME = "";
      public const int    DEFAULT_RF_232_BAUD = 9600;
*/
      public const string DEFAULT_WARD_LIST_FILE = "";
      
      private const string KEY_SOFTWARE = "Software";
      private const string KEY_CHERRY_HILL_9TH = "Cherry_Hill_9th";
      private const string KEY_MAP = "MAP";
      private const string KEY_SETTINGS = "Settings";


       private const string VALUE_WARD_LIST_FILE = "WardListFile";
       private const string VALUE_NOT_ON_WARD_LIST_FILE = "NotOnWardListFile";

       private string wardListFile = "";
       private string notOnWardListFile = "";

      //====================================================================
      // ProgramSettings()
      //
      //   Constructor
      //
      //
      //=====================================================================
      public ProgramSettings()
      {
         InitSettingsFromRegistry();
      }

      //====================================================================
      // WardListFile()
      //
      //   wardListFile property
      //=====================================================================
       public string WardListFile
      {
         get
         {
             return wardListFile;
         }
      }
      //====================================================================
      // NotOnWardListFile()
      //
      //   notOnWardListFile property
      //=====================================================================
      public string NotONWardListFile
      {
          get
          {
              return notOnWardListFile;
          }
      }
      //====================================================================
      // SaveWardListFile()
      //
      //   Saves settings from the Db Form to the registry and this class
      //
      //
      //=====================================================================
       public void SaveWardListFile(string wardListFileName)
      {
         RegistryKey settingsKey = GetRegistrySettingsKey();

         wardListFile = wardListFileName;

         settingsKey.SetValue(VALUE_WARD_LIST_FILE, wardListFile, RegistryValueKind.String);
      }
      //====================================================================
      // SaveWardListFile()
      //
      //   Saves settings from the Db Form to the registry and this class
      //
      //
      //=====================================================================
       public void SaveNotOnWardListFile(string notOnardListFileName)
      {
          RegistryKey settingsKey = GetRegistrySettingsKey();

          notOnWardListFile = notOnardListFileName;

          settingsKey.SetValue(VALUE_NOT_ON_WARD_LIST_FILE, notOnWardListFile, RegistryValueKind.String);
 

      }
      //====================================================================
      // LoadWardListFileName()
      //
      //   Gets db options from the registry
      //
      //
      //=====================================================================
      public void LoadWardListFileName(string wardListFileName)
      {
          RegistryKey settingsKey = GetRegistrySettingsKey();
         wardListFileName = wardListFile;
      }
      //====================================================================
      // LoaNotOnWardListFileName()
      //
      //   Gets notOnWardList file name from the registry
      //
      //
      //=====================================================================
      public void LoadNotOnWardListFileName(string notOnWardListFileName)
      {
          RegistryKey settingsKey = GetRegistrySettingsKey();
          //notOnwardListFileName = notOnwardListFile;
      }

      //====================================================================
      // SaveSettingsFormOptions()
      //
      //   Saves settings from the SettingsForm to the registry and this class
      //
      //
      //=====================================================================
 
      //====================================================================
      // InitSettingsFromRegistry
      //
      //   Initalizes settings from the registry
      //
      //
      //=====================================================================
      private void InitSettingsFromRegistry()
      {
         RegistryKey settingsKey = GetRegistrySettingsKey();

         wardListFile = settingsKey.GetValue(VALUE_WARD_LIST_FILE).ToString();
         notOnWardListFile = settingsKey.GetValue(VALUE_NOT_ON_WARD_LIST_FILE).ToString();

         //turnAroundTime = int.Parse(settingsKey.GetValue(VALUE_TURNAROUND_TIME).ToString());
         //Properties.Settings.Default.Rf232Baud = rf232Baud;
      }


      //====================================================================
      // GetRegistrySettingsKey()
      //
      //   Gets the settings key, or creates and sets defaults if it does not exist
      //
      //
      //=====================================================================
       private RegistryKey GetRegistrySettingsKey()
      {
         bool foundKey = false;
         int numKeys;
         int loopCntr;
         /*

         KEY_SOFTWARE = "Sof
         KEY_CHERRY_HILL_9TH
         KEY_MAP = "MAP";
         KEY_SETTINGS = "Set

         */



         RegistryKey regKey = Registry.CurrentUser;
         String[] keyNames = regKey.GetSubKeyNames();
         numKeys = regKey.SubKeyCount;

         for (loopCntr = 0; loopCntr < numKeys; loopCntr++)
         {
            if (keyNames[loopCntr] == KEY_SOFTWARE)
            {
               foundKey = true;
               break;
            }
         }

         if (foundKey == false)
         {
            regKey.CreateSubKey(KEY_SOFTWARE);
         }

         RegistryKey softwareKey = regKey.OpenSubKey(KEY_SOFTWARE, true);
         keyNames = softwareKey.GetSubKeyNames();
         numKeys = softwareKey.SubKeyCount;
         foundKey = false;

         // find the cherry hill 9th key
        for (loopCntr = 0; loopCntr < numKeys; loopCntr++)
        {
            if (keyNames[loopCntr] == KEY_CHERRY_HILL_9TH)
            {
                foundKey = true;
                break;
            }
        }// end find cherry hill 9th
        
        if (foundKey == false)
        {
            softwareKey.CreateSubKey(KEY_CHERRY_HILL_9TH);
        }

        RegistryKey waveKey = softwareKey.OpenSubKey(KEY_CHERRY_HILL_9TH, true);
         keyNames = waveKey.GetSubKeyNames();
         numKeys = waveKey.SubKeyCount;
         foundKey = false;

         // find the map key
         for (loopCntr = 0; loopCntr < numKeys; loopCntr++)
         {
             if (keyNames[loopCntr] == KEY_MAP)
            {
               foundKey = true;
               break;
            }
         }// end find SS125_DspManTest

         if (foundKey == false)
         {
             waveKey.CreateSubKey(KEY_MAP);
         }

         RegistryKey testerKey = waveKey.OpenSubKey(KEY_MAP, true);
         keyNames = testerKey.GetSubKeyNames();
         numKeys = testerKey.SubKeyCount;
         foundKey = false;

         // find the settings key
         for (loopCntr = 0; loopCntr < numKeys; loopCntr++)
         {
            if (keyNames[loopCntr] == KEY_SETTINGS)
            {
               foundKey = true;
               break;
            }
         }// end find settings

         if (foundKey == false)
         {
            testerKey.CreateSubKey(KEY_SETTINGS);
         }

         RegistryKey settingsKey = testerKey.OpenSubKey(KEY_SETTINGS, true);

         string[] valueNames = settingsKey.GetValueNames();
         int numValues = settingsKey.ValueCount;
         bool foundWardListFileName = false;
         bool foundNotOnWardListFileName = false;

         for (loopCntr = 0; loopCntr < numValues; loopCntr++)
         {
             if (valueNames[loopCntr] == VALUE_WARD_LIST_FILE)
            {
                foundWardListFileName = true;
            }
            else if (valueNames[loopCntr] == VALUE_NOT_ON_WARD_LIST_FILE)
            {
                foundNotOnWardListFileName = true;
            }
         }

         if (foundWardListFileName == false)
         {
             settingsKey.SetValue(VALUE_WARD_LIST_FILE, "", RegistryValueKind.String);
         }
         if (foundNotOnWardListFileName == false)
         {
             settingsKey.SetValue(VALUE_NOT_ON_WARD_LIST_FILE, "", RegistryValueKind.String);
         }
         return (settingsKey);
      }
   }
}
