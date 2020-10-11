﻿using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.Filter;
using System;
using System.Collections.Generic;
using MarioPizzaOriginal.Domain.DataAccess;
using MarioPizzaOriginal.Domain.Enums;
using MarioPizzaOriginal.Tools;
using TinyIoC;

namespace MarioPizzaOriginal.Controller
{
    public class IngredientController
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IngredientFilter _ingredientFilter;
        private readonly MenuCreator _ingredientMenu;
        private readonly ViewHelper _viewHelper;
        private readonly IConsole _console;
        public IngredientController()
        {
            var container = TinyIoCContainer.Current;
            _console = container.Resolve<IConsole>();
            _ingredientRepository = container.Resolve<IIngredientRepository>();
            _viewHelper = new ViewHelper(_console);
            _ingredientFilter = new IngredientFilter(_console);
            _ingredientMenu = MenuCreator.Create()
                .SetHeader("Dostępne opcje - składniki: ")
                .AddOptionRange(new Dictionary<string, Action>
                {
                    {"Wszystkie dostępne składniki", GetAllIngredients},
                    {"Szczegóły składnika", GetIngredient},
                    {"Dodaj składnik", AddIngredient},
                    {"Edytuj składnik", EditIngredient},
                    {"Usuń składnik", DeleteIngredient},
                    {"Filtruj", GetFilteredIngredients}
                })
                .AddFooter("Powrót");
        }

        public void IngredientsMenu() => _ingredientMenu.Present();

        public void AddIngredient()
        {
            var newIngredient = new Ingredient
            {
                IngredientName = _viewHelper.AskForStringNotBlank("Podaj nazwę nowego składnika: "),
                UnitOfMeasureType = _viewHelper.AskForOption<UnitOfMeasure>("Dostępne jednostki miary:", "Podaj numer jednostki miary: ")
            };

            _console.WriteLine("Wpisz -1 jeżeli chcesz pozostawić pole puste");
            var priceSmall = _viewHelper.AskForString("Podaj cenę (mały rozmiar): ");
            if (!priceSmall.Equals("-1")) 
                newIngredient.PriceSmall = Convert.ToDouble(priceSmall.Replace(".", ","));

            var priceMedium = _viewHelper.AskForString("Podaj cenę (średni rozmiar): ");
            if (!priceMedium.Equals("-1")) 
                newIngredient.PriceMedium = Convert.ToDouble(priceMedium.Replace(".", ","));

            var priceLarge = _viewHelper.AskForString("Podaj cenę (duży rozmiar): ");
            if (!priceLarge.Equals("-1")) 
                newIngredient.PriceLarge = Convert.ToDouble(priceLarge.Replace(".", ","));

            _ingredientRepository.Add(newIngredient);
            _viewHelper.WriteAndWait($"Dodano składnik: {newIngredient.IngredientName}");
        }

        public void GetAllIngredients()
        {
            _console.Clear();
            _console.WriteLine("Lista wszystkich składników:");
            ShowIngredients(_ingredientRepository.GetAll());
            _console.ReadLine();
        }

        public void EditIngredient()
        {
            if (CheckIfIngredientNotExists("Podaj id składnika który chcesz modyfikować: ", out var ingredientId))
                return;

            Ingredient actual = _ingredientRepository.Get(ingredientId);
            string ingredientName = _viewHelper.AskForString($"Podaj nazwę składnika ({actual.IngredientName}): ");
            if (!ingredientName.Equals(""))
            {
                actual.IngredientName = ingredientName;
            }

            actual.UnitOfMeasureType = _viewHelper.AskForOption<UnitOfMeasure>("Dostępne jednostki miary: ", $"Jednostka miary ({actual.UnitOfMeasureType}): ", actual.UnitOfMeasureType.ToString());

            _console.WriteLine("Wpisz -1 by ustawić wartość NULL");
            string priceSmall = _viewHelper.AskForString($"Podaj cenę (mały rozmiar) ({actual.PriceSmall}): ");
            if (priceSmall.Equals("-1")) { actual.PriceSmall = null; }
            else if (!priceSmall.Equals("")) { actual.PriceSmall = Convert.ToDouble(priceSmall.Replace(",", ".")); }

            string priceMedium = _viewHelper.AskForString($"Podaj cenę (średni rozmiar) ({actual.PriceMedium}): ");
            if (priceMedium.Equals("-1")) { actual.PriceMedium = null; }
            else if (!priceMedium.Equals("")) { actual.PriceMedium = Convert.ToDouble(priceMedium.Replace(",", ".")); }

            string priceLarge = _viewHelper.AskForString($"Podaj cenę (duży rozmiar) ({actual.PriceLarge}): ");
            if (priceLarge.Equals("-1")) { actual.PriceLarge = null; }
            else if (!priceLarge.Equals("")) { actual.PriceLarge = Convert.ToDouble(priceLarge.Replace(",", ".")); }

            _ingredientRepository.Save(actual);
        }

        public void DeleteIngredient()
        {
            if (CheckIfIngredientNotExists("Podaj id składnika który chcesz usunąć: ", out var ingredientId))
                return;
            _ingredientRepository.Remove(ingredientId);
            _viewHelper.WriteAndWait($"Usunięto składnik o id {ingredientId}");
        }

        public void GetIngredient()
        {
            if (CheckIfIngredientNotExists("Podaj id składnika: ", out var ingredientId)) 
                return;
            var ingredient = _ingredientRepository.Get(ingredientId);
            DescribeIngredient(ingredient);
        }

        public void GetFilteredIngredients()
        {
            _ingredientFilter.Clear();
            if (_ingredientFilter.FilterMenu())
            {
                var results = _ingredientFilter.Query();
                ShowIngredients(results);
                _viewHelper.WriteAndWait($"Znaleziono {results.Count} pasujących do filtra:");
            }
        }


        private void ShowIngredients(List<Ingredient> ingredients)
        {
            _console.Clear();
            string header = $"{"Id",5}|{"Nazwa składnika",30}|{"Jednostka miary",15}|";
            _console.WriteLine(new string('=', header.Length));
            ingredients.ForEach(x =>
            {
                _console.WriteLine($"{x.IngredientId,5}|{x.IngredientName,30}|{x.UnitOfMeasureType,15}|");
            });
        }

        private void DescribeIngredient(Ingredient ingredient)
        {
            var text = new List<string> {
                $"Nazwa składnika: {ingredient.IngredientName}",
                $"Numer porządkowy: {ingredient.IngredientId}",
                $"Jednostka miary: {ingredient.UnitOfMeasureType}",
                $"Cena (Mała): {ingredient.PriceSmall}",
                $"Cena (Średnia): {ingredient.PriceMedium}",
                $"Cena (Duża): {ingredient.PriceLarge}"};
            text.ForEach(_console.WriteLine);
            _console.ReadLine();
        }
        private bool CheckIfIngredientNotExists(string message, out int ingredientId)
        {
            ingredientId = _viewHelper.AskForInt(message);
            if (_ingredientRepository.Exists(ingredientId))
                return false;
            _viewHelper.WriteAndWait($"Składnik o numerze {ingredientId} nie istnieje!");
            return true;
        }
    }
}
