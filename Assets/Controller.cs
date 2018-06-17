using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BoxCollider2D))]
public class Controller : MonoBehaviour {


    public LayerMask colliderMask;
    // Corner pts of the player
    struct CornerPoints
    {
        public Vector2 topLeft, topRight, bottomLeft, bottomRight;

    }

    public struct collisionInformation
    {
        public bool left, right, above, below,climbingSlope, des_slope;
       public  float oldSlopeAngle, newSlopeAngle;
       public Vector2 newVelocity,oldVelocity;

        public void Reset()
        {
            left = right = above = below =climbingSlope= des_slope = false;
            oldSlopeAngle = newSlopeAngle;
            newSlopeAngle = 0;
            oldVelocity = newVelocity;
        }
    }


    BoxCollider2D playerCollider;
    CornerPoints cornerpts;
    const float skinMargin = 0.015f;
    // Number of rays to cast
    int horizontalRayCastCount = 4;
    int verticalRayCastCount = 4;
    // Distance b/w the rays
    float horizontalRaySpacing;
    float verticalRaySpacing;

    float maxSlopeClimbAngle = 80f;
    float maxDescendAngle = 75f;

    public collisionInformation col_info;

    

	
	void Start () {
        playerCollider = GetComponent<BoxCollider2D>();
        calculateRaySpacing();
    }
	

    // Collision handling
    public void Move(Vector2 velocity)
    {

        updateCornerPoints();
        col_info.Reset();

        // Descending the slope
        if(velocity.y < 0)
        {
            DescendSlope(ref velocity);
        }
       
        if (velocity.x != 0)
        {
            horizontalCollisions(ref velocity);
        }
        if (velocity.y != 0)
        {
            verticalCollision(ref velocity);
        }
        transform.Translate(velocity);
        
    }

    public void verticalCollision(ref Vector2 velocity)
    {
        float y_direction = Mathf.Sign(velocity.y);
        float raylength = Mathf.Abs(velocity.y) + skinMargin;
        Vector2 rayOrigin;
       
        for (int i = 0; i < verticalRayCastCount; i++)
        {
            // Moving Down
            if (y_direction == -1)
            {
                rayOrigin = cornerpts.bottomLeft;
            }// Moving up
            else
            {
                rayOrigin = cornerpts.topLeft;
            }

            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            // Hitting the ray for collision detection
            RaycastHit2D rayHit = Physics2D.Raycast(rayOrigin, y_direction *Vector2.up, raylength, colliderMask);
            Debug.DrawRay(cornerpts.bottomLeft + Vector2.right * verticalRaySpacing * i, Vector2.up * -3, Color.blue);

            if (rayHit)
            {
                velocity.y = (rayHit.distance - skinMargin) * y_direction; // Distance from current point to point of ray collision
                raylength = rayHit.distance; // Updating ray length
                // Direction of where the collision occurred
                col_info.below = y_direction == -1;
                col_info.above = y_direction == 1;
            }
        }

    }

    public void horizontalCollisions(ref Vector2 velocity)
    {
        float x_direction = Mathf.Sign(velocity.x);
        float raylength = Mathf.Abs(velocity.x) + skinMargin;
        Vector2 rayOrigin;

        for (int i = 0; i < horizontalRayCastCount; i++)
        {
            // Moving Down
            if (x_direction == -1)
            {
                rayOrigin = cornerpts.bottomLeft;
            }// Moving up
            else
            {
                rayOrigin = cornerpts.bottomRight;
            }

            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            // Hitting the ray for collision detection
            RaycastHit2D rayHit = Physics2D.Raycast(rayOrigin, x_direction * Vector2.right, raylength, colliderMask);


            if (rayHit)
            {

                float slopeAngle = Vector2.Angle(rayHit.normal, Vector2.up);        // Inclination Angle
               
                // Bottom most right ray will detect the slope
                if(i==0 && slopeAngle <= maxSlopeClimbAngle)
                {
                   
                    AscendSlope(ref velocity, slopeAngle);
                }

                // If not Climbing, only then check rest of the rays for the collision
                if (!col_info.climbingSlope || slopeAngle > maxSlopeClimbAngle)
                {
                    velocity.x = (rayHit.distance - skinMargin) * x_direction; // Distance from current point to point of ray collision
                    raylength = rayHit.distance; // Updating ray length

                    col_info.left = x_direction == -1;
                    col_info.right = x_direction == 1;
                }
            }
        }

    }


    void AscendSlope(ref Vector2 velocity, float slopeAngle)
    {
        float hypotenus = Mathf.Abs(velocity.x);
        float perpendicular_distance = hypotenus * Mathf.Sin(slopeAngle * Mathf.Deg2Rad);
        // Not Jump
        if (perpendicular_distance >= velocity.y)
        {
            velocity.y = perpendicular_distance;
            velocity.x = hypotenus * Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
            // Reducing the speed while climbing
            velocity.x /= 8;

            col_info.below = true;
            col_info.climbingSlope = true;
            col_info.newSlopeAngle = slopeAngle;

        }
       
    }

    void DescendSlope(ref Vector2 velocity)
    {
        float x_dir = Mathf.Sign(velocity.x);
        // If moving left, ray originated from bottom left, otherwise bottomright
        Vector2 rayStart = (x_dir == -1) ? cornerpts.bottomRight : cornerpts.bottomLeft;
        // Originate ray from start, move downward infinitely as we don't know what each slope perimeters are
        RaycastHit2D rayHit = Physics2D.Raycast(rayStart, -Vector2.up, Mathf.Infinity, colliderMask);

        if (rayHit)
        {
            float slopeAngle = Vector2.Angle(rayHit.normal, Vector2.up);
            if(slopeAngle !=0 && slopeAngle <= maxDescendAngle)
            {
                if(Mathf.Sign(rayHit.normal.x) == x_dir)
                {
                    if(rayHit.distance - skinMargin <= (Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))){
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = moveDistance * Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);

                        velocity.y -= descendVelY;

                        col_info.newSlopeAngle = slopeAngle;
                        col_info.des_slope = true;
                        col_info.below = true;

                        col_info.newVelocity = velocity;

                    }
                }
            }
        }
    }

    void updateCornerPoints()
    {
        Bounds bound = playerCollider.bounds;
        // the player bounds are shrinked
        bound.Expand(skinMargin * -2);
        // Updating all the corner pts
        cornerpts.bottomLeft = new Vector2(bound.min.x, bound.min.y);
        cornerpts.bottomRight = new Vector2(bound.max.x, bound.min.y);
        cornerpts.topLeft = new Vector2(bound.min.x, bound.max.y);
        cornerpts.topRight = new Vector2(bound.max.x, bound.max.y);

    }

    void calculateRaySpacing()
    {
        Bounds bound = playerCollider.bounds;
        bound.Expand(skinMargin * -2);

        // Atleast 2 rays fired
        horizontalRayCastCount = Mathf.Clamp(horizontalRayCastCount, 2, int.MaxValue);
        verticalRayCastCount = Mathf.Clamp(verticalRayCastCount, 2, int.MaxValue);

        // Distance b/w rays based on the size of player and how many rays were fired
        horizontalRaySpacing = bound.size.y / (horizontalRayCastCount - 1);
        verticalRaySpacing = bound.size.x / (verticalRayCastCount - 1);
    }
}
