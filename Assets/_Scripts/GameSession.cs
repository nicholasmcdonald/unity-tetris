﻿using System.Collections;
    {
        GameObject[] text = GameObject.FindGameObjectsWithTag("Text");
        foreach (GameObject obj in text)
            obj.GetComponent<MeshRenderer>().sortingLayerName = "Text";
    }