using Objects.Global;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Maps
{
    public class Map
    {
        int roomSize = 10;
        int connectorSize = 10;
        int connectorThickeness = 2;

        private IZone _zone;

        public Map(IZone zone)
        {
            _zone = zone;
        }

        public void GenerateMap()
        {
            MapGrid grid = GenerateMapGrid();
            CenterGrid(grid);
            DrawGrid(grid);
            BuildRoomToPositionConversion(grid, _zone.Id);
        }

        private MapGrid GenerateMapGrid()
        {
            MapGrid grid = new MapGrid();
            grid.BuildRooms(_zone);
            return grid;
        }

        private void CenterGrid(MapGrid grid)
        {
            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int minZ = int.MaxValue;
            foreach (MapRoom room in grid.Grid.Values)
            {
                minX = Math.Min(minX, room.Position.X);
                minY = Math.Min(minY, room.Position.Y);
                minZ = Math.Min(minZ, room.Position.Z);
            }

            foreach (MapRoom room in grid.Grid.Values)
            {
                room.Position.X += 1 - minX;
                room.Position.Y += 1 - minY;
                room.Position.Z += 1 - minZ;
            }
        }

        public void DrawGrid(MapGrid grid)
        {
            int maxX = 0;
            int maxY = 0;
            int maxZ = 0;

            foreach (MapRoom room in grid.Grid.Values)
            {
                maxX = Math.Max(maxX, room.Position.X);
                maxY = Math.Max(maxY, room.Position.Y);
                maxZ = Math.Max(maxZ, room.Position.Z);
            }

            int xDimension = maxX * (roomSize + connectorSize) + connectorSize;
            int yDimension = maxY * (roomSize + connectorSize) + connectorSize;

            for (int z = 1; z <= maxZ; z++)
            {
                using (Image<Rgba32> image = new Image<Rgba32>(xDimension, yDimension))
                {
                    DrawRooms(grid, image, z);
                    DrawConnections(grid, image, z);

                    int zone = 0;
                    foreach (var item in grid.Grid.Keys)
                    {
                        zone = item.Zone;
                        break;
                    }

                    string fileName = Path.Combine(GlobalReference.GlobalValues.Settings.AssetsDirectory, "Maps", string.Format(@"{0}-{1}.png", zone, z));
                    image.Save(fileName);
                }
            }
        }

        private void DrawConnections(MapGrid grid, Image<Rgba32> image, int z)
        {
            int maxHeight = image.Height;

            foreach (IRoom room in grid.Grid.Keys)
            {
                MapRoom mapRoom = grid.Grid[room];
                if (mapRoom.Position.Z == z)
                {
                    int xRoomPos = RoomPosition(mapRoom.Position.X);
                    int yRoomPos = RoomPosition(mapRoom.Position.Y);
                    if (room.North != null)
                    {
                        for (int x = 0; x < connectorThickeness; x++)
                        {
                            int xPos = xRoomPos + x + 4;

                            for (int y = 0; y < connectorSize / 2; y++)
                            {
                                int yPos = yRoomPos + roomSize + y;

                                image[xPos, ReverseY(yPos, image)] = Rgba32.Black;
                            }
                        }
                    }

                    if (room.East != null)
                    {
                        for (int x = 0; x < connectorSize / 2; x++)
                        {
                            int xPos = (xRoomPos + roomSize) + x;

                            for (int y = 0; y < connectorThickeness; y++)
                            {
                                int yPos = yRoomPos + y + 4;

                                image[xPos, ReverseY(yPos, image)] = Rgba32.Black;
                            }
                        }
                    }

                    if (room.South != null)
                    {
                        for (int x = 0; x < connectorThickeness; x++)
                        {
                            int xPos = xRoomPos + x + 4;

                            for (int y = 0; y < connectorSize / 2; y++)
                            {
                                int yPos = yRoomPos - y - 1;

                                image[xPos, ReverseY(yPos, image)] = Rgba32.Black;
                            }
                        }
                    }

                    if (room.West != null)
                    {
                        for (int x = 0; x < connectorSize / 2; x++)
                        {
                            int xPos = xRoomPos - x - 1;

                            for (int y = 0; y < connectorThickeness; y++)
                            {
                                int yPos = yRoomPos + y + 4;

                                image[xPos, ReverseY(yPos, image)] = Rgba32.Black;
                            }
                        }
                    }

                    if (room.Up != null)
                    {
                        for (int x = 0; x < connectorSize / 2; x++)
                        {
                            int xPos = xRoomPos + roomSize - 1 + x;
                            int yPos = yRoomPos + roomSize + x;

                            for (int i = 0; i < 2; i++)
                            {
                                image[xPos + i, ReverseY(yPos, image)] = Rgba32.Black;
                            }

                        }
                    }

                    if (room.Down != null)
                    {
                        for (int x = 0; x < connectorSize / 2; x++)
                        {
                            int xPos = xRoomPos - x;
                            int yPos = yRoomPos - x;
                            for (int i = 0; i < 2; i++)
                            {
                                image[xPos - i, ReverseY(yPos, image)] = Rgba32.Black;
                            }
                        }
                    }
                }
            }
        }

        private void DrawRooms(MapGrid grid, Image<Rgba32> image, int z)
        {
            foreach (MapRoom room in grid.Grid.Values)
            {
                if (room.Position.Z == z)
                {
                    for (int x = 0; x < roomSize; x++)
                    {
                        int xPos = RoomPosition(room.Position.X) + x;

                        for (int y = 0; y < roomSize; y++)
                        {
                            int yPos = RoomPosition(room.Position.Y) + y;

                            image[xPos, ReverseY(yPos, image)] = Rgba32.Black;
                        }
                    }
                }
            }
        }

        private int RoomPosition(int roomPosition)
        {
            return (roomPosition - 1) * (roomSize + connectorSize) + connectorSize;
        }

        private int ReverseY(int y, Image<Rgba32> image)
        {
            //not sure why but off by 1 so we offset here
            return image.Height - y - 1;
        }

        public void BuildRoomToPositionConversion(MapGrid grid, int id)
        {
            using (TextWriter tw = new StreamWriter(Path.Combine(GlobalReference.GlobalValues.Settings.AssetsDirectory, "Maps", string.Format("{0}.MapConversion", id))))
            {
                foreach (IRoom room in grid.Grid.Keys)
                {
                    Position pos = grid.Grid[room].Position;
                    tw.WriteLine($"{room.Id}|{pos.Z}|{RoomPosition(pos.X)}|{RoomPosition(pos.Y)}");
                }
            }
        }
    }
}
