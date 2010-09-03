using System;
using Locom.Modules.Common.RecordSet;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Collections.Generic;

/*
* $Header: /cvs/PlanIt/PlanItComponents/PlanItManager/ServiceAccess/Client.cs,v 1.5 2010/04/14 14:33:15 intranet+alexanderk Exp $
*
* $Log: Client.cs,v $
*
*/

namespace Locom.PlanIt.PlanItManager.ServiceAccess
{
    [Serializable]
    public sealed class {TABLE.NAME} : IMasterData, IComparable, IComparable<{TABLE.NAME}>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="{TABLE.NAME}"/> class.
        /// </summary>
        /// <param name="record">The {TABLE.NAME} record.</param>
        public {TABLE.NAME}(IRecord record)
        {
        {TABLE.COLUMNS}
            this._{COLUMN.NAME} = Convert.To{MAP COLUMN.TYPE}(record[PlanItConsts.{TABLE.NAME}.{COLUMN.NAME}]);
        {/TABLE.COLUMNS}
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="{TABLE.NAME}"/> class.
        /// </summary>
        public Client(
            {TABLE.COLUMNS}
                {MAP COLUMN.TYPE} {COLUMN.NAME},
            {/TABLE.COLUMNS}
        )
        {
            {TABLE.COLUMNS}
                this._{COLUMN.NAME} = {COLUMN.NAME};
            {/TABLE.COLUMNS}
        }


        #region Properties

        {TABLE.COLUMNS PRIMARY}
        public int {COLUMN.NAME}
        {
            [DebuggerHidden]
            get { return this._{COLUMN.NAME}; }
        }
        private readonly int _{COLUMN.NAME};	
        {/TABLE.COLUMNS}

        {TABLE.COLUMNS NOPRIMARY}
        public string {COLUMN.NAME}
        {
            [DebuggerHidden]
            get { return this._{COLUMN.NAME}; }
            [DebuggerHidden]
            set { this._{COLUMN.NAME} = value; }
        }
        private string _{COLUMN.NAME};
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
           		record[PlanItConsts.{TABLE.NAME}.{COLUMN.NAME}] = this._{COLUMN.NAME};
		{/TABLE.COLUMNS}
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

            {TABLE.NAME} tmp = other as {TABLE.NAME};
            if (null == tmp)
                return false;

            return this.id.Equals(tmp.Id);
        }

        /// <summary>
        /// Fungiert als Hashfunktion für einen bestimmten Typ. <see cref="M:System.Object.GetHashCode"></see> eignet sich für die Verwendung in Hashalgorithmen und Hashdatenstrukturen, z. B. in einer Hashtabelle.
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
            return this.name;
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
            return this.CompareTo(other as {TABLE.NAME});
        }

        #endregion

        #region IComparable<{TABLE.NAME}> Member

        /// <summary>
        /// Vergleicht das aktuelle Objekt mit einem anderen Objekt desselben Typs.
        /// </summary>
        /// <param name="other">Ein Objekt, das mit diesem Objekt verglichen werden soll.</param>
        /// <returns>
        /// Eine 32-Bit-Ganzzahl mit Vorzeichen, die die relative Reihenfolge der verglichenen Objekte angibt. Der Rückgabewert hat folgende Bedeutung:WertBedeutung Kleiner als 0 (null)Dieses Objekt ist kleiner als der other-Parameter.0 Dieses Objekt ist gleich other. Größer als 0 (null)Dieses Objekt ist größer als other.
        /// </returns>
        public int CompareTo({TABLE.NAME} other)
        {
            if (null == other)
                return -1;

            //TODO: spezifische Vergleichsfunktion fuer {TABLE.NAME} implementieren
            throw new NotImplementedException();
        }

        #endregion
    }
}
