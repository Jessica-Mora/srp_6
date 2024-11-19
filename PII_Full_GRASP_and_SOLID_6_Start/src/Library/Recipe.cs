//-------------------------------------------------------------------------
// <copyright file="Recipe.cs" company="Universidad Católica del Uruguay">
// Copyright (c) Programación II. Derechos reservados.
// </copyright>
//-------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Full_GRASP_And_SOLID
{
    public class Recipe : IRecipeContent, TimerClient // Modificado por DIP
    //public class Recipe :TimerClient
    {
        // Cambiado por OCP
        private IList<BaseStep> steps = new List<BaseStep>();
        
        // inici bool
        private bool cooked = false;

        public Product FinalProduct { get; set; }

        // Agregado por Creator
        public void AddStep(Product input, double quantity, Equipment equipment, int time)
        {
            Step step = new Step(input, quantity, equipment, time);
            this.steps.Add(step);
        }

        // Agregado por OCP y Creator
        public void AddStep(string description, int time)
        {
            WaitStep step = new WaitStep(description, time);
            this.steps.Add(step);
        }

        public void RemoveStep(BaseStep step)
        {
            this.steps.Remove(step);
        }

        // Agregado por SRP
        public string GetTextToPrint()
        {
            string result = $"Receta de {this.FinalProduct.Description}:\n";
            foreach (BaseStep step in this.steps)
            {
                result = result + step.GetTextToPrint() + "\n";
            }

            // Agregado por Expert
            result = result + $"Costo de producción: {this.GetProductionCost()}";

            return result;
        }

        // Agregado por Expert
        public double GetProductionCost()
        {
            double result = 0;

            foreach (BaseStep step in this.steps)
            {
                result = result + step.GetStepCost();
            }

            return result;
        }

        public int GetCookTime() //SRP porque asigna la responsabilidad a la clase que contiene ,a informacion
        {
            int total_time = 0;
            foreach (var step in this.steps)
            {
                total_time += step.Time;
            }
            return total_time;  
        }
        
        public bool Cooked
        {
            get{ return this.Cooked; }
        }
        public void Cook() //Creator ya que esta clase crea instancias de otras clases
        {
            int cooking_Time = this.GetCookTime();
            CountdownTimer timercd = new CountdownTimer();
            timercd.Register(cooking_Time , this);
        }

        public void TimeOut()
        {
            this.cooked = true;
            
        }
    }
}
