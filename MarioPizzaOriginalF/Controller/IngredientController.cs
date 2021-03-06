﻿using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.Filter;
using System;
using System.Collections.Generic;
using MarioPizzaOriginal.Domain.DataAccess;
using MarioPizzaOriginal.Domain.Enums;
using TinyIoC;

namespace MarioPizzaOriginal.Controller
{
    public class IngredientController
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IngredientFilter _ingredientFilter;
        private readonly MenuCreator _ingredientMenu;
        public IngredientController(TinyIoCContainer container)
        {
            _ingredientRepository = container.Resolve<IIngredientRepository>();
            _ingredientFilter = new IngredientFilter(container);
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
            Ingredient newIngredient = new Ingredient
            {
                IngredientName = ViewHelper.AskForStringNotBlank("Podaj nazwę nowego składnika: "),
                UnitOfMeasureType = ViewHelper.AskForOption<UnitOfMeasure>("Dostępne jednostki miary:", "Podaj numer jednostki miary: ")
            };

            Console.WriteLine("Wpisz -1 jeżeli chcesz pozostawić pole puste");
            var priceSmall = ViewHelper.AskForString("Podaj cenę (mały rozmiar): ");
            if (!priceSmall.Equals("-1")) 
                newIngredient.PriceSmall = Convert.ToDouble(priceSmall.Replace(".", ","));

            var priceMedium = ViewHelper.AskForString("Podaj cenę (średni rozmiar): ");
            if (!priceMedium.Equals("-1")) 
                newIngredient.PriceMedium = Convert.ToDouble(priceMedium.Replace(".", ","));

            var priceLarge = ViewHelper.AskForString("Podaj cenę (duży rozmiar): ");
            if (!priceLarge.Equals("-1")) 
                newIngredient.PriceLarge = Convert.ToDouble(priceLarge.Replace(".", ","));

            _ingredientRepository.Add(newIngredient);
            ViewHelper.WriteAndWait($"Dodano składnik: {newIngredient.IngredientName}");
        }

        public void GetAllIngredients()
        {
            Console.Clear();
            Console.WriteLine("Lista wszystkich składników:");
            ShowIngredients(_ingredientRepository.GetAll());
            Console.ReadLine();
        }

        public void EditIngredient()
        {
            if (CheckIfIngredientNotExists("Podaj id składnika który chcesz modyfikować: ", out var ingredientId))
                return;

            Ingredient actual = _ingredientRepository.Get(ingredientId);
            string ingredientName = ViewHelper.AskForString($"Podaj nazwę składnika ({actual.IngredientName}): ");
            if (!ingredientName.Equals(""))
            {
                actual.IngredientName = ingredientName;
            }

            actual.UnitOfMeasureType = ViewHelper.AskForOption<UnitOfMeasure>("Dostępne jednostki miary: ", $"Jednostka miary ({actual.UnitOfMeasureType}): ", actual.UnitOfMeasureType.ToString());

            Console.WriteLine("Wpisz -1 by ustawić wartość NULL");
            string priceSmall = ViewHelper.AskForString($"Podaj cenę (mały rozmiar) ({actual.PriceSmall}): ");
            if (priceSmall.Equals("-1")) { actual.PriceSmall = null; }
            else if (!priceSmall.Equals("")) { actual.PriceSmall = Convert.ToDouble(priceSmall.Replace(",", ".")); }

            string priceMedium = ViewHelper.AskForString($"Podaj cenę (średni rozmiar) ({actual.PriceMedium}): ");
            if (priceMedium.Equals("-1")) { actual.PriceMedium = null; }
            else if (!priceMedium.Equals("")) { actual.PriceMedium = Convert.ToDouble(priceMedium.Replace(",", ".")); }

            string priceLarge = ViewHelper.AskForString($"Podaj cenę (duży rozmiar) ({actual.PriceLarge}): ");
            if (priceLarge.Equals("-1")) { actual.PriceLarge = null; }
            else if (!priceLarge.Equals("")) { actual.PriceLarge = Convert.ToDouble(priceLarge.Replace(",", ".")); }

            _ingredientRepository.Save(actual);
        }

        public void DeleteIngredient()
        {
            if (CheckIfIngredientNotExists("Podaj id składnika który chcesz usunąć: ", out var ingredientId))
                return;
            _ingredientRepository.Remove(ingredientId);
            ViewHelper.WriteAndWait($"Usunięto składnik o id {ingredientId}");
        }


        private void ShowIngredients(List<Ingredient> ingredients)
        {
            string header = $"{"Id",5}|{"Nazwa składnika",30}|{"Jednostka miary",15}|";
            Console.WriteLine(new string('=', header.Length));
            ingredients.ForEach(x =>
            {
                Console.WriteLine($"{x.IngredientId,5}|{x.IngredientName,30}|{x.UnitOfMeasureType,15}|");
            });
        }

        private void DescribeIngredient(Ingredient ingredient)
        {
            List<string> text = new List<string> { 
                $"Nazwa składnika: {ingredient.IngredientName}",
                $"Numer porządkowy: {ingredient.IngredientId}",
                $"Jednostka miary: {ingredient.UnitOfMeasureType}",
                $"Cena (Mała): {ingredient.PriceSmall}",
                $"Cena (Średnia): {ingredient.PriceMedium}",
                $"Cena (Duża): {ingredient.PriceLarge}"};
            text.ForEach(Console.WriteLine);
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
                Console.WriteLine($"Znaleziono {results.Count} pasujących do filtra:");
                ShowIngredients(results);
            }
        }


        private bool CheckIfIngredientNotExists(string message, out int ingredientId)
        {
            ingredientId = ViewHelper.AskForInt(message);
            if (_ingredientRepository.Exists(ingredientId))
                return false;
            ViewHelper.WriteAndWait($"Składnik o numerze {ingredientId} nie istnieje!");
            return true;
        }
    }
}
