using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientPresentation.Commands
{
    /// <summary>
    /// This is the Abstract class that sets the base for Commandes
    /// It's used when definig new commandes
    /// </summary>
    public abstract class CommandBase : ICommand
    {
        /// <summary>
        /// Defines the CanExecute method for the CommanedBase, 
        /// used to sett a Enable or Disabeled on a object
        /// </summary>
        /// <param name="parameter"> Either a object that is sent to the Command or null</param>
        /// <returns> Bool </returns>
        public abstract bool CanExecute(object? parameter);

        /// <summary>
        /// Defines the Execute method for the CommanedBase
        /// </summary>
        /// <param name="parameter"> Either a object that is sent to the Command or null</param>
        /// <returns> void </returns>
        public abstract void Execute(object? parameter);

        /// <summary>
        /// Defines the CanExecuteChanged EventHandeler
        /// Manages changes of the return value of canExecute
        /// </summary>
        /// <returns> event </returns>
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
