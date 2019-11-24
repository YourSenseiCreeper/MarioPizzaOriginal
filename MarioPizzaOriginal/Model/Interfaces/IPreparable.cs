using System;
using System.Collections.Generic;
using System.Text;

namespace MarioPizzaOriginal.Model.Interfaces
{
    public interface IPreparable
    {
        void Prepare();
        List<string> GetPreparationInstructions();
        List<Ingredient> GetIngredients();
    }
}
