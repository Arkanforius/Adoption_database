﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data;
using System.Windows.Forms;

namespace AdoptionDatabase
{
    //This class consists exclusively of static methods
    //The methods either serve as intermediary methods to the DatabaseInterface class, or they are general helper methods like hasContents().

    class Info
    {
      
        //This method constructs a SQL query to get entries in the pet table that meet the given criteria.
        //It returns all of the data in the form of a linked list of Record objects, using a ReturnedDataHolder object as the head.
        public static ReturnedDataHolder queryPetsForUser(string[] petTypes, string petGender, string ageOperator, string age)
        {
            ReturnedDataHolder pets = new ReturnedDataHolder();
            string query = "SELECT PET.PET_ID, PETSHOP.NAME, AGENCY.NAME, VET.NAME, PET.PET_NAME, PET.PET_TYPE, PET.PICTURE, PET.GENDER, PET.AGE, PET.ADOPTION_PRICE FROM (PET JOIN PETSHOP ON(PET.PETSHOP_ID = PETSHOP.PETSHOP_ID) JOIN VET ON VET.VET_ID = PET.VET_ID JOIN AGENCY ON AGENCY.AGENCY_ID = PET.AGENCY_ID)";

            bool followingOutside = false;
            bool followingInside = false;

            if (hasContents(petTypes))
            {
                if (followingOutside == false)
                    query += " WHERE";

                query += "(";

                for (int i = 0; i < petTypes.Length; i++)
                {
                    if (petTypes[i] != null)
                    {
                        if (followingInside)
                            query += " OR";

                        query = query + " (PET.PET_TYPE = '" + petTypes[i] + "')";

                        followingInside = true;
                    }


                }
                followingInside = false;
                followingOutside = true;

                query += " )";
            }

            if (petGender != null)
            {
                if (followingOutside == false)
                {
                    query += " WHERE";
                }
                else
                {
                    query += " AND";
                }

                query += " (PET.GENDER = '" + petGender + "')";

                followingOutside = true;

            }

            if (ageOperator != "")
            {
                if (followingOutside == false)
                    query += " WHERE";
                else
                {
                    query += " AND";
                }

                query += " (PET.AGE " + ageOperator + " " + age + ")";

                followingOutside = true;
            }

            query += ";";

            Debug.WriteLine(query);

            DatabaseInterface DataLink = new DatabaseInterface();

            pets = DataLink.queryDatabase(query, 10);

            pets.dumpToConsole();



            return pets;
        }



        //This is a genral helper method that returns true if an array of strings has at least one non-null entry.
        public static bool hasContents(string[] s)
        {

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] != null)
                {
                    return true;
                }

            }
            return false;

        }


        //The searchX methods take a TextBox object, construct an appropriate SQL query to search the given table,
        //and call the requestTable method in DatabaseInterface to get and return a DataTable object.

        public static DataTable searchPet(string[] infoBox, TextBox text)
        {
            DatabaseInterface activeInterface = new DatabaseInterface();
            string whereString;
            string selectString = "SELECT P.PET_ID, P.PETSHOP_ID, P.AGENCY_ID, P.VET_ID, P.PET_NAME, P.PET_TYPE, P.PICTURE, P.GENDER, AGE, P.ADOPTION_PRICE, AD.FIRSTNAME FROM PET AS P LEFT OUTER JOIN ADOPTION AS A ON P.PET_ID = A.PET_ID LEFT OUTER JOIN ADOPTER AS AD ON AD.ADOPTER_ID = A.ADOPTER_ID WHERE P.PET_ID LIKE '%" + text.Text + "%' OR P.PETSHOP_ID LIKE '%" + text.Text + "%' OR P.AGENCY_ID LIKE '%" + text.Text + "%' OR P.PET_NAME LIKE '%" + text.Text + "%' OR P.VET_ID LIKE '%" + text.Text + "%' OR P.PET_TYPE LIKE '%" + text.Text + "%' OR P.PICTURE LIKE '%" + text.Text + "%' OR P.GENDER LIKE '%" + text.Text + "%' OR AGE LIKE '%" + text.Text + "%' OR P.ADOPTION_PRICE LIKE '%" + text.Text + "%' OR AD.FIRSTNAME LIKE '%" + text.Text + "%'";

            if (infoBox == null)
                whereString = "";
            else
            {
                whereString = "";
            }

            return activeInterface.requestTable(selectString, whereString);
        }
        public static DataTable searchAgency(string[] infoBox, TextBox text)
        {
            DatabaseInterface activeInterface = new DatabaseInterface();
            string whereString;
            string selectString = "SELECT * FROM AGENCY WHERE AGENCY_ID LIKE '%" + text.Text + "%' OR NAME LIKE '%" + text.Text + "%' OR ADDRESS LIKE '%" + text.Text + "%' OR PHONE_NUM LIKE '%" + text.Text + "%' OR LOGO LIKE '%" + text.Text + "%'";

            if (infoBox == null)
                whereString = "";
            else
            {
                whereString = "";
            }

            return activeInterface.requestTable(selectString, whereString);
        }
        public static DataTable searchVet(string[] infoBox, TextBox text)
        {
            DatabaseInterface activeInterface = new DatabaseInterface();
            string whereString;
            string selectString = "SELECT * FROM VET WHERE VET_ID LIKE '%" + text.Text + "%' OR NAME LIKE '%" + text.Text + "%' OR ADDRESS LIKE '%" + text.Text + "%' OR PHONE_NUM LIKE '%" + text.Text + "%' OR LOGO LIKE '%" + text.Text + "%'";

            if (infoBox == null)
                whereString = "";
            else
            {
                whereString = "";
            }

            return activeInterface.requestTable(selectString, whereString);
        }
        public static DataTable searchShop(string[] infoBox, TextBox text)
        {
            DatabaseInterface activeInterface = new DatabaseInterface();
            string whereString;
            string selectString = "SELECT * FROM PETSHOP WHERE PETSHOP_ID LIKE '%" + text.Text + "%' OR NAME LIKE '%" + text.Text + "%' OR ADDRESS LIKE '%" + text.Text + "%' OR PHONE_NUM LIKE '%" + text.Text + "%' OR LOGO LIKE '%" + text.Text + "%'";

            if (infoBox == null)
                whereString = "";
            else
            {
                whereString = "";
            }

            return activeInterface.requestTable(selectString, whereString);
        }
        public static DataTable searchAdopter(string[] infoBox, TextBox text)
        {
            DatabaseInterface activeInterface = new DatabaseInterface();
            string whereString;
            string selectString = "SELECT AD.ADOPTER_ID, AD.FIRSTNAME, AD.LASTNAME, AD.PHONE, AD.ADDRESS, AD.CITY, AD.STATE, AD.ZIP, AP.PET_ID, T.START_TIME, ADOPTION.PET_ID FROM(ADOPTER AS AD LEFT OUTER JOIN APPOINTMENT AS AP ON AD.ADOPTER_ID = AP.ADOPTER_ID) LEFT OUTER JOIN APPOINTMENT_SLOT AS APS ON((AP.TIMESLOT_ID = APS.TIMESLOT_ID) AND(AP.VOLUNTEER_ID = APS.VOLUNTEER_ID)) LEFT OUTER JOIN TIMESLOT AS T ON T.TIMESLOT_ID = APS.TIMESLOT_ID LEFT OUTER JOIN ADOPTION ON AD.ADOPTER_ID = ADOPTION.ADOPTER_ID WHERE AD.ADOPTER_ID LIKE '%" + text.Text + "%' OR AD.FIRSTNAME LIKE '%" + text.Text + "%' OR AD.LASTNAME LIKE '%" + text.Text + "%' OR AD.PHONE LIKE '%" + text.Text + "%' OR AD.ADDRESS LIKE '%" + text.Text + "%' OR AD.CITY LIKE '%" + text.Text + "%' OR AD.STATE LIKE '%" + text.Text + "%' OR AD.ZIP LIKE '%" + text.Text + "%' OR AP.PET_ID LIKE '%" + text.Text + "%' OR T.START_TIME LIKE '%" + text.Text + "%' OR ADOPTION.PET_ID LIKE '%" + text.Text + "%'";

            if (infoBox == null)
                whereString = "";
            else
            {
                whereString = "";
            }

            return activeInterface.requestTable(selectString, whereString);
        }
        public static DataTable searchVolunteer(string[] infoBox, TextBox text)
        {
            DatabaseInterface activeInterface = new DatabaseInterface();
            string whereString;
            string selectString = "SELECT VOLUNTEER_ID, FIRSTNAME, LASTNAME, IS_ADMIN FROM VOLUNTEER WHERE VOLUNTEER_ID LIKE '%" + text.Text + "%' OR FIRSTNAME LIKE '%" + text.Text + "%' OR LASTNAME LIKE '%" + text.Text + "%' OR IS_ADMIN LIKE '%" + text.Text + "%'";
            if (infoBox == null)
                whereString = "";
            else
            {
                whereString = "";
            }

            return activeInterface.requestTable(selectString, whereString);
        }
        public static DataTable searchAppointment(string[] infoBox, TextBox text)
        {
            DatabaseInterface activeInterface = new DatabaseInterface();
            string whereString;
            string selectString = "SELECT A.APPOINTMENT_ID, AD.FIRSTNAME, P.PET_NAME, V.FIRSTNAME, T.TIMESLOT_ID, T.START_TIME, T.END_TIME FROM APPOINTMENT AS A JOIN APPOINTMENT_SLOT AS APS ON((A.VOLUNTEER_ID = APS.VOLUNTEER_ID) AND(A.TIMESLOT_ID = APS.TIMESLOT_ID)) JOIN TIMESLOT AS T ON(APS.TIMESLOT_ID = T.TIMESLOT_ID) JOIN VOLUNTEER AS V ON APS.VOLUNTEER_ID = V.VOLUNTEER_ID JOIN ADOPTER AS AD ON AD.ADOPTER_ID = A.ADOPTER_ID JOIN PET AS P ON P.PET_ID = A.PET_ID WHERE A.APPOINTMENT_ID LIKE '%" + text.Text + "%' OR AD.FIRSTNAME LIKE '%" + text.Text + "%' OR P.PET_NAME LIKE '%" + text.Text + "%' OR V.FIRSTNAME LIKE '%" + text.Text + "%' OR T.TIMESLOT_ID LIKE '%" + text.Text + "%' OR T.START_TIME LIKE '%" + text.Text + "%' OR T.END_TIME LIKE '%" + text.Text + "%';";
            if (infoBox == null)
                whereString = "";
            else
            {
                whereString = "";
            }

            return activeInterface.requestTable(selectString, whereString);
        }
    

        //The getTable methods return a DataTable representation of the given table by querying requestTable in DatabaseInterface
        public static DataTable getPetTable(string[] infoBox)
        {
            DatabaseInterface activeInterface = new DatabaseInterface();
            string whereString;
            string selectString = "SELECT P.PET_ID, P.PETSHOP_ID, P.AGENCY_ID, P.VET_ID, P.PET_NAME, P.PET_TYPE, P.PICTURE, P.GENDER, AGE, P.ADOPTION_PRICE, AD.FIRSTNAME FROM PET AS P LEFT OUTER JOIN ADOPTION AS A ON P.PET_ID = A.PET_ID LEFT OUTER JOIN ADOPTER AS AD ON AD.ADOPTER_ID = A.ADOPTER_ID";
;
            if (infoBox == null)                   
                whereString = "";
            else
            {
                whereString = "";
            }

            return activeInterface.requestTable(selectString, whereString);
        }
        public static DataTable getAgencyTable(string[] infoBox)
        {
            DatabaseInterface activeInterface = new DatabaseInterface();
            string whereString;
            string selectString = "SELECT AGENCY_ID, NAME, ADDRESS, PHONE_NUM, LOGO FROM AGENCY";
            if (infoBox == null)
                whereString = "";
            else
            {
                whereString = "";
            }

            return activeInterface.requestTable(selectString, whereString);
        }
        public static DataTable getVetTable(string[] infoBox)
        {
            DatabaseInterface activeInterface = new DatabaseInterface();
            string whereString;
            string selectString = "SELECT V.VET_ID, V.NAME, V.ADDRESS, V.PHONE_NUM, V.LOGO FROM VET AS V JOIN SPECIALIZATIONS_has_VET AS SV ON V.VET_ID = SV.VET_VET_ID JOIN SPECIALIZATIONS AS S ON S.ID = SV.SPECIALIZATIONS_ID";
            if (infoBox == null)
                whereString = "";
            else
            {
                bool followingInside = false;

                whereString = " WHERE ";

                for (int i = 0; i < infoBox.Length; i++)
                {
                    if (infoBox[i] != null)
                    {
                        if (followingInside)
                            whereString += " OR";

                        whereString = whereString + " (S.NAME = '" + infoBox[i] + "')";

                        followingInside = true;
                    }


                }
            }

            whereString += " GROUP BY VET_ID;";

            return activeInterface.requestTable(selectString, whereString);
        }
        public static DataTable getShopTable(string[] infoBox)
        {
            DatabaseInterface activeInterface = new DatabaseInterface();
            string whereString;
            string selectString = "SELECT PETSHOP_ID, NAME, ADDRESS, PHONE_NUM, LOGO FROM PETSHOP";
            if (infoBox == null)
                whereString = "";
            else
            {
                whereString = "";
            }

            return activeInterface.requestTable(selectString, whereString);
        }
        public static DataTable getAdopterTable(string[] infoBox)
        {
            DatabaseInterface activeInterface = new DatabaseInterface();
            string whereString;
            string selectString = "SELECT AD.ADOPTER_ID, AD.FIRSTNAME, AD.LASTNAME, AD.PHONE, AD.ADDRESS, AD.CITY, AD.STATE, AD.ZIP, AP.PET_ID, T.START_TIME, ADOPTION.PET_ID FROM(ADOPTER AS AD LEFT OUTER JOIN APPOINTMENT AS AP ON AD.ADOPTER_ID = AP.ADOPTER_ID) LEFT OUTER JOIN APPOINTMENT_SLOT AS APS ON((AP.TIMESLOT_ID = APS.TIMESLOT_ID) AND(AP.VOLUNTEER_ID = APS.VOLUNTEER_ID)) LEFT OUTER JOIN TIMESLOT AS T ON T.TIMESLOT_ID = APS.TIMESLOT_ID LEFT OUTER JOIN ADOPTION ON AD.ADOPTER_ID = ADOPTION.ADOPTER_ID";
            if (infoBox == null)
                whereString = "";
            else
            {
                whereString = "";
            }

            return activeInterface.requestTable(selectString, whereString);
        }
        public static DataTable getVolunteerTable(string[] infoBox)
        {
            DatabaseInterface activeInterface = new DatabaseInterface();
            string whereString;
            string selectString = "SELECT VOLUNTEER_ID, FIRSTNAME, LASTNAME, IS_ADMIN FROM VOLUNTEER";
            if (infoBox == null)
                whereString = "";
            else
            {
                whereString = "";
            }

            return activeInterface.requestTable(selectString, whereString);
        }
        public static DataTable getAppointmentTable(string[] infoBox)
        {
            DatabaseInterface activeInterface = new DatabaseInterface();
            string whereString;
            string selectString = "SELECT A.APPOINTMENT_ID, AD.FIRSTNAME, P.PET_NAME, V.FIRSTNAME, T.TIMESLOT_ID, T.START_TIME, T.END_TIME FROM APPOINTMENT AS A JOIN APPOINTMENT_SLOT AS APS ON((A.VOLUNTEER_ID = APS.VOLUNTEER_ID) AND(A.TIMESLOT_ID = APS.TIMESLOT_ID)) JOIN TIMESLOT AS T ON(APS.TIMESLOT_ID = T.TIMESLOT_ID) JOIN VOLUNTEER AS V ON APS.VOLUNTEER_ID = V.VOLUNTEER_ID JOIN ADOPTER AS AD ON AD.ADOPTER_ID = A.ADOPTER_ID JOIN PET AS P ON P.PET_ID = A.PET_ID;";
            if (infoBox == null)
                whereString = "";
            else
            {
                whereString = "";
            }

            return activeInterface.requestTable(selectString, whereString);
        }


        //The getIndex method is used to find the ID number of the associated data in its given table. It calls the getIndex method in DatabaseInterface.
        public static int getIndex(string database, string data)
        {
            
            DatabaseInterface activeInterface = new DatabaseInterface();

            switch(database)
            {
                case "ADOPTER":
                    return activeInterface.getIndex("ADOPTER", "ADOPTER_ID", data, "FIRSTNAME");
                case "PET":
                    return activeInterface.getIndex("PET", "PET_ID", data, "PET_NAME");
                default:
                    return activeInterface.getIndex("VOLUNTEER", "VOLUNTEER_ID", data, "FIRSTNAME");


            }



        }



        //This is an intermediary method that takes a SQL query and calls the modifyDatabase method in DatabaseInterface.
        public static void modifyDatabase(string query)
        {
            DatabaseInterface activeInterface = new DatabaseInterface();
            activeInterface.modifyDatabase(query);
        }

    }

    
   
    
}
