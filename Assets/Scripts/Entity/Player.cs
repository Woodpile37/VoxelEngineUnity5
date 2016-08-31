using UnityEngine;
using System.Collections;

public class Player : Entity {

    FirstPersonController fpsController;

    Block selectedBlock;

    Direction selectedDirection;

    public float playerReachableDistance = 10;

    GameObject fpsObject;

    public float handBlockBreakingPower = 0.1f;

    Inventory currentInventory;

    public Inventory playerInventory;

    override
    public void init() {
        fpsObject = transform.Find("FirstPersonCharacter").gameObject as GameObject;
        if (playerInventory == null) {
            playerInventory = new Inventory(name + "'s Inventory", name);
        }
    }
    Player() {

    }
    Player(string name) {
        entityName = name;
        fpsController = GetComponent<FirstPersonController>();
    }
    public FirstPersonController getController() {
        return fpsController;
    }
    override
       public Location getEyeLocation() {
        return new Location(transform.position + fpsObject.transform.position);
    }
    override
        public Vector3 getDirection() {
        return fpsObject.transform.rotation * Vector3.forward;
    }
    public void selectBlock(Block block, Direction direction) {
        selectedBlock = block;
        selectedDirection = direction;
        Vector3 pos = selectedBlock.getLocation().add(selectedDirection).toVector();
        //Debug.Log("selectedBlock: " + selectedBlock.getLocation().toVector() + " +  " + selectedDirection.toVector() + selectedDirection.ToString() + "  = " + pos);
        GameManager.getBlockSelectionHandler().show(pos);
    }
    public void deselectBlock() {
        GameManager.getBlockSelectionHandler().hide();
        selectedBlock = null;
    }
    public Block getSelectedBlock() {
        return selectedBlock;
    }
    public Direction getSelectedDirection() {
        return selectedDirection;
    }
    public float getBlockBreakingPower() {
        return handBlockBreakingPower;
    }
    void Update() {
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out hit, 100)) {
            GameObject go = hit.collider.gameObject;
            if (go.tag.Equals("Chunk")) {
                Location l = new Location(hit.point);
                Block block = l.getPointedBlockLocation(ray.direction).getBlock();
                selectBlock(block, l.getNearestDirectionInBlock(block));
                //Debug.Log(block.getLocation().toVector() + "  " + hit.point + l.getNearestDirectionInBlock(block));
            } else {
                deselectBlock();
            }
        } else {
            deselectBlock();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            if (getSelectedBlock() == null) {
                PlayerInteractEvent e = new PlayerInteractEvent(this, PlayerInteractEvent.Action.LEFT_CLICK_AIR, getSelectedBlock(), getSelectedDirection());
                EventRegistry.registerPlayerInteractEvent(e);
            } else {
                PlayerInteractEvent e = new PlayerInteractEvent(this, PlayerInteractEvent.Action.LEFT_CLICK_BLOCK, getSelectedBlock(), getSelectedDirection());
                EventRegistry.registerPlayerInteractEvent(e);
            }
        } else if (Input.GetKeyDown(KeyCode.Mouse1)) {
            if (getSelectedBlock() == null) {
                PlayerInteractEvent e = new PlayerInteractEvent(this, PlayerInteractEvent.Action.RIGHT_CLICK_AIR, getSelectedBlock(), getSelectedDirection());
                EventRegistry.registerPlayerInteractEvent(e);
            } else {
                PlayerInteractEvent e = new PlayerInteractEvent(this, PlayerInteractEvent.Action.RIGHT_CLICK_BLOCK, getSelectedBlock(), getSelectedDirection());
                EventRegistry.registerPlayerInteractEvent(e);
            }
        }
    }
    public void onPlayerInteractEvent(PlayerInteractEvent e) {
        Block block = e.getBlock();
        if (e.getAction().Equals(PlayerInteractEvent.Action.LEFT_CLICK_BLOCK)) {
            block.setType(SkyMaterial.AIR);
        } else if (e.getAction().Equals(PlayerInteractEvent.Action.RIGHT_CLICK_BLOCK)) {
            block.getRelative(e.getClickedSide()).setType(SkyMaterial.DIRT);
        }

    }
    public Inventory getInventory() {
        return this.playerInventory;
    }
    public void onOpenInventoryKeyPress() {
        if (isInInventory()) {
            closeInventory();
        } else {
            openInventory(getInventory());
        }
    }
    public void openInventory(Inventory inv) {
        currentInventory = inv;
        inv.setItem(0, new ItemStack("hehe"));
        inv.setItem(4, new ItemStack("hehe"));
        inv.setItem(6, new ItemStack("hehe"));
        InterfaceHandler.showInventory(inv);
    }
    public bool isInInventory() {
        if (currentInventory == null) {
            return false;
        } else {
            return true;
        }
    }
    public void closeInventory() {
        currentInventory = null;
        InterfaceHandler.closeInventory();
    }
}
