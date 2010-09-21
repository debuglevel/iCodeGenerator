using System;
using Locom.Modules.Common.RecordSet;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

/*
* $Header: /cvs/PlanIt/PlanItComponents/PlanItManager/ServiceAccess/Client.cs,v 1.5 2010/04/14 14:33:15 intranet+alexanderk Exp $
*
* $Log: Client.cs,v $
*
*/


namespace Locom.PlanIt.Masterdata.ServiceAccess.{TABLE.NAME REMOVEPREFIX_LOWER_FIRSTUPPER}
{
    [Serializable]
    public sealed class {TABLE.NAME REMOVEPREFIX_LOWER_FIRSTUPPER}Model : IRecordSerializable, IComparable, IComparable<{TABLE.NAME REMOVEPREFIX_LOWER_FIRSTUPPER}Model>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="{TABLE.NAME REMOVEPREFIX_LOWER_FIRSTUPPER}"/> class.
        /// </summary>
        /// <param name="record">The {TABLE.NAME} record.</param>
        public {TABLE.NAME REMOVEPREFIX_LOWER_FIRSTUPPER}Model(IRecord record)
        {
        {TABLE.COLUMNS}
        {IF COLUMN.NAME !~ '(UPD|INS)_(USER|DATE)'}	this.{COLUMN.NAME REMOVEPREFIX_LOWER} = Convert.To{MAP COLUMN.TYPE}(record[Masterdata{TABLE.NAME REMOVEPREFIX_LOWER_FIRSTUPPER}Consts.ModuleTable.{TABLE.NAME REMOVEPREFIX_UPPER}_{COLUMN.NAME REMOVEPREFIX_UPPER}.Name]);{/IF}
	 {/TABLE.COLUMNS}
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="{TABLE.NAME REMOVEPREFIX_LOWER_FIRSTUPPER}"/> class.
        /// </summary>
        public {TABLE.NAME REMOVEPREFIX_LOWER_FIRSTUPPER}Model(
            {TABLE.COLUMNS}
		{IF COLUMN.NAME !~ '(UPD|INS)_(USER|DATE)'}	
                {MAP COLUMN.TYPE} {COLUMN.NAME REMOVEPREFIX_LOWER}{IF NOT LAST},{/IF}{/IF}{/TABLE.COLUMNS}
        )
        {
            {TABLE.COLUMNS}
		{IF COLUMN.NAME !~ '(UPD|INS)_(USER|DATE)'}	
                this.{COLUMN.NAME REMOVEPREFIX_LOWER} = {COLUMN.NAME REMOVEPREFIX_LOWER};{/IF}{/TABLE.COLUMNS}
        }


        #region Properties

        {TABLE.COLUMNS PRIMARY}
        public {MAP COLUMN.TYPE} {COLUMN.NAME REMOVEPREFIX_LOWER_FIRSTUPPER}
        {
            [DebuggerHidden]
            get { return this.{COLUMN.NAME REMOVEPREFIX_LOWER}; }
        }
        private readonly {MAP COLUMN.TYPE} {COLUMN.NAME REMOVEPREFIX_LOWER};	
        {/TABLE.COLUMNS}

        {TABLE.COLUMNS NOPRIMARY}
	 {IF COLUMN.NAME !~ '(UPD|INS)_(USER|DATE)'}	
        public {MAP COLUMN.TYPE} {COLUMN.NAME REMOVEPREFIX_LOWER_FIRSTUPPER}
        {
            [DebuggerHidden]
            get { return this.{COLUMN.NAME REMOVEPREFIX_LOWER}; }
            [DebuggerHidden]
            set { this.{COLUMN.NAME REMOVEPREFIX_LOWER} = value; }
        }
        private {MAP COLUMN.TYPE} {COLUMN.NAME REMOVEPREFIX_LOWER};
	 {/IF}
        {/TABLE.COLUMNS}

        #endregion


        #region IRecordSerializable Member

        /// <summary>
        ///  Converts this client to its record representation form
        /// </summary>
        public void ToRecord(IRecord record)
        {
            if (null == record)
                throw new ArgumentNullException();

		{TABLE.COLUMNS}
		{IF COLUMN.NAME !~ '(UPD|INS)_(USER|DATE)'}	
           		record[Masterdata{TABLE.NAME REMOVEPREFIX_LOWER_FIRSTUPPER}Consts.ModuleTable.{TABLE.NAME REMOVEPREFIX_UPPER}_{COLUMN.NAME REMOVEPREFIX_UPPER}.Name] = this.{COLUMN.NAME REMOVEPREFIX_LOWER};{/IF}{/TABLE.COLUMNS}
        }

        #endregion

        #region Interface

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public override bool Equals(object other)
        {
            if (null == other)
                return false;

            {TABLE.NAME REMOVEPREFIX_LOWER_FIRSTUPPER}Model tmp = other as {TABLE.NAME REMOVEPREFIX_LOWER_FIRSTUPPER}Model;
            if (null == tmp)
                return false;

            return this.id.Equals(tmp.Id);
        }

        /// <summary>
        /// Fungiert als Hashfunktion für einen bestimmten Typ. <see cref="M:System.Object.GetHashCode"></see> eignet sich für die Verwendung in Hashalgorithmen und Hashdatenstrukturen, z. B. in einer Hashtabelle.
        /// </summary>
        /// <returns>
        /// Ein Hashcode für das aktuelle <see cref="T:System.Object"></see>.
        /// </returns>
        public override int GetHashCode()
        {
            return this.id;
        }

        /// <summary>
        /// Gibt einen <see cref="T:System.String"></see> zurück, der den aktuellen <see cref="T:System.Object"></see> darstellt.
        /// </summary>
        /// <returns>
        /// Ein <see cref="T:System.String"></see>, der den aktuellen <see cref="T:System.Object"></see> darstellt.
        /// </returns>
        public override string ToString()
        {
            //TODO: spezifische ToString-Funktion implementieren
            throw new NotImplementedException();
        }

        #endregion

        #region IComparable Member

        /// <summary>
        /// Compares the name of this instance to the specified object.
        /// </summary>
        /// <param name="other">The object to compare.</param>
        /// <returns></returns>
        public int CompareTo(object other)
        {
            return this.CompareTo(other as {TABLE.NAME REMOVEPREFIX_LOWER_FIRSTUPPER}Model);
        }

        #endregion

        #region IComparable<{TABLE.NAME REMOVEPREFIX_LOWER_FIRSTUPPER}Model> Member

        /// <summary>
        /// Vergleicht das aktuelle Objekt mit einem anderen Objekt desselben Typs.
        /// </summary>
        /// <param name="other">Ein Objekt, das mit diesem Objekt verglichen werden soll.</param>
        /// <returns>
        /// Eine 32-Bit-Ganzzahl mit Vorzeichen, die die relative Reihenfolge der verglichenen Objekte angibt. Der Rückgabewert hat folgende Bedeutung:WertBedeutung Kleiner als 0 (null)Dieses Objekt ist kleiner als der other-Parameter.0 Dieses Objekt ist gleich other. Größer als 0 (null)Dieses Objekt ist größer als other.
        /// </returns>
        public int CompareTo({TABLE.NAME REMOVEPREFIX_LOWER_FIRSTUPPER}Model other)
        {
            if (null == other)
                return -1;

            //TODO: spezifische Vergleichsfunktion fuer {TABLE.NAME REMOVEPREFIX_LOWER_FIRSTUPPER} implementieren
            throw new NotImplementedException();
        }

        #endregion
    }
}

