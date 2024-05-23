using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewExperimentFlow", menuName = "Experiment/Experiment Flow")]
public class ExperimentFlow : ScriptableObject
{
    public List<ExperimentStep> Steps;
}
