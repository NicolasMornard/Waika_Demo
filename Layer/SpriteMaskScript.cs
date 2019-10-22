using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]

//Type of mask --> reference to SpriteMask.Shader
public enum MaskType { OFF, GONE, RED, GREEN, BLUE, CYAN, MAGENTA, YELLOW, BLACK, WHITE, NEGATIVE, GRAY, FADE};

[ExecuteInEditMode]

public class SpriteMaskScript : MonoBehaviour
{
    /***
     * This script must be attach to all the component you want to be affect by the mask
     * Renderer : Material --> SpriteMask
     * Renderer : Additional Layer : Sorting Layer --> same as the other object affected
     * Parameters:
     *          - Type : type of the mask
     *          - Distance : circle ray
     *          - Position Y Hide : if it is the Avatar, let it to 0
     *                              if it is an object the avatar can turn around,
     *                                  this parameter correspond to at the value add to the y of the pivot
     *                                  this coordinate is used to know from which height the mask is activated                   
     ***/

    private BoxCollider2D BoxCollider;
    private float PosY;

    private SpriteRenderer spriteRenderer;
    private TilemapRenderer tileRenderer;

    private int tmp = 0;
    private float tmpDistance;

    private bool up = true;
    private float smoothDistance;

    public MaskType type = MaskType.OFF;
    public float Distance = 5;
    public static GameObject Target;

    public float PositionYHide;

    public bool FireEffect = false;
    public int FireRange = 20;

    public bool SmoothEffect = false;
    public int SmoothRange;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) tileRenderer = GetComponent<TilemapRenderer>();
        //Target is always the mask
        if (Target == null) Target = GameObject.FindGameObjectWithTag("mask");

        //collider of the object
        BoxCollider = GetComponent<BoxCollider2D>();
        //pivot
        PosY = GetComponent<Transform>().position.y + PositionYHide;

        smoothDistance = Distance;
    }

    
    // Update is called once per frame
    void Update()
    {
        //if (target == null) target = GameObject.FindGameObjectWithTag("TestTag");
        updateShader();
        //toggleShader();
        //Define Distance taking into account max and min
        Distance = Mathf.Clamp(Distance, 1, 500);

        if (SmoothEffect && FireEffect) FireEffect = false;
    }

    private int typeToInt()
    {
        if (Distance <= 0 || type == MaskType.OFF) return 0;
        switch (type)
        {
            case MaskType.GONE: return 1;
            case MaskType.RED: return 2;
            case MaskType.GREEN: return 3;
            case MaskType.BLUE: return 4;
            case MaskType.CYAN: return 5;
            case MaskType.MAGENTA: return 6;
            case MaskType.YELLOW: return 7;
            case MaskType.BLACK: return 8;
            case MaskType.WHITE: return 9;
            case MaskType.NEGATIVE: return 10;
            case MaskType.GRAY: return 11;
            case MaskType.FADE: return 12;
        }
        return 0;
    }
    private void toggleShader()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0)) type = MaskType.OFF;
        if (Input.GetKeyDown(KeyCode.Keypad1)) type = MaskType.GONE;
        if (Input.GetKeyDown(KeyCode.Keypad2)) type = MaskType.RED;
        if (Input.GetKeyDown(KeyCode.Keypad3)) type = MaskType.GREEN;
        if (Input.GetKeyDown(KeyCode.Keypad4)) type = MaskType.BLUE;
        if (Input.GetKeyDown(KeyCode.Keypad5)) type = MaskType.CYAN;
        if (Input.GetKeyDown(KeyCode.Keypad6)) type = MaskType.MAGENTA;
        if (Input.GetKeyDown(KeyCode.Keypad7)) type = MaskType.YELLOW;
        if (Input.GetKeyDown(KeyCode.Keypad8)) type = MaskType.BLACK;
        if (Input.GetKeyDown(KeyCode.Keypad9)) type = MaskType.WHITE;
        if (Input.GetKeyDown(KeyCode.I)) type = MaskType.NEGATIVE;
        if (Input.GetKeyDown(KeyCode.O)) type = MaskType.GRAY;
        if (Input.GetKeyDown(KeyCode.P)) type = MaskType.FADE;
    }

    private void updateShader()
    {
        //if there are no renderer
        if (spriteRenderer == null && tileRenderer == null || Target == null) return;

        MaterialPropertyBlock mpb = new MaterialPropertyBlock();

        //get property of the renderer
        if (spriteRenderer != null) spriteRenderer.GetPropertyBlock(mpb);
        if (tileRenderer != null) tileRenderer.GetPropertyBlock(mpb);

        //change value of the shader
        if (FireEffect) mpb.SetFloat("_RenderDistance", CalculRandomDistance());
        else if (SmoothEffect) mpb.SetFloat("_RenderDistance", CalculSmoothDistance());
        else mpb.SetFloat("_RenderDistance", Distance);
        
        mpb.SetFloat("_MaskTargetX", Target.transform.position.x);
        mpb.SetFloat("_MaskTargetY", Target.transform.position.y);
        mpb.SetFloat("_MaskType", typeToInt());

        //update shader
        if (spriteRenderer != null) spriteRenderer.SetPropertyBlock(mpb);
        if (tileRenderer != null) tileRenderer.SetPropertyBlock(mpb);
    }

    //When the collider of the object is trigger
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Avatar>())
        {
            //if the avatar is before the point
            if (GameDirector.Avatar.transform.position.y > PosY)
            {
                type = MaskType.FADE;
            }
            else
            {
                type = MaskType.OFF;
            }

        }
    }

    //when the avatar quit the object
    void OnTriggerExit2D(Collider2D other)
    {
        type = MaskType.OFF;
    }

    float CalculRandomDistance()
    {
        if (tmp == 0)
        {
            tmp = Random.Range(0, 10);
            float per = Random.Range(0, FireRange);
            tmpDistance = Distance + (Distance * per / 100);
            return tmpDistance;
        }
        else
        {
            tmp--;
            return tmpDistance;
        }
            
    }

    float CalculSmoothDistance()
    {
        if (smoothDistance > Distance + SmoothRange) up = false;
        if (smoothDistance < Distance) up = true;
        if (up) smoothDistance+=0.2f;
        else smoothDistance-=0.2f;

        return smoothDistance;

    }


}
